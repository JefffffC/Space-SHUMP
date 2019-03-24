using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;


    [Header("Set in the Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set Dynamically")]
    [SerializeField] // forces Unity to still show _shieldLevel despite being private
    private float _shieldLevel = 1;

    public delegate void WeaponFireDelegate(); // declare a new delegate type WeaponFireDelegate
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
      // fireDelegate += TempFire;
    }


    // Start is called before the first frame update
    void Start()
    {

        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Start() - Attempted to Assign Second Hero! ");

        }

    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //use the fireDelegate to fire weapons, make sure button is pressed, then ensure fireDelegate isn't null to avoid error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null) // either space or up arrow
        {
            fireDelegate();
        }

 
    }


    private GameObject lastTriggerGo = null; // this variable holds a reference to the last triggering GameObject
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered: " + go.name);

        // make sure it's not triggering go as last time
        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;

        if (go.tag == "Enemy") // only triggers if shield collides with enemy
        {
            --shieldLevel;
            Destroy(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public float shieldLevel // property
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4); // ensure _shieldLevel is never set to higher than 4
            // if the shield is going to be set to less than zero, then Hero is destroyed
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }

    }

}