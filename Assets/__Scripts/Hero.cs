using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Weapon activeWeapon;
    public GameObject coolDownBox;




    [Header("Set Dynamically")]
    [SerializeField] // forces Unity to still show _shieldLevel despite being private
    private float _shieldLevel = 1;
    private GameObject _activeCooldownBox1; // private variables for active powerup timers
    private GameObject _activeCooldownBox2;
    private bool _countingdownBox1 = false;
    private bool _countingdownBox2 = false;

    public delegate void WeaponFireDelegate(); // declare a new delegate type WeaponFireDelegate
    public WeaponFireDelegate fireDelegate;

    private AudioSource shieldDownSound; //sound for when shields are down

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
        shieldDownSound = GetComponent<AudioSource>(); //initializing the sound

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

        if (_countingdownBox1 == true)
        {
            if (_activeCooldownBox1 != null)
            {
                if (_activeCooldownBox1.GetComponent<CooldownManager>().coolingDown == false)
                {
                    //disable invincibility
                    _countingdownBox1 = false;
                }
            }
        }
        if (_countingdownBox2 == true)
        {
            if (_activeCooldownBox2 != null)
            {
                if (_activeCooldownBox2.GetComponent<CooldownManager>().coolingDown == false)
                {
                    activeWeapon.SetType(activeWeapon.defaultWeapon);
                    _countingdownBox2 = false;
                }
            }
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
            shieldDownSound.Play(); //play sound of shield destroyed
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp (GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.BonusLife:
                Debug.Log("BonusLife PowerUp collected");
                break;
            case WeaponType.Invincible:
                Debug.Log("Invincible PowerUp Collected");

                // insert code for Invincible stuff here

                _activeCooldownBox1 = Instantiate<GameObject>(coolDownBox); // instantiate CoolDownBox from prefab
                _activeCooldownBox1.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false); // move it under canvas so it displays properly
                _activeCooldownBox1.GetComponent<CooldownManager>().activateCooldown(pu.type); // get the script of the cooldown and activate it, passing powerup type 
                _countingdownBox1 = true; // begin tracking cooldown for deactivation of powerup
                break;
            case WeaponType.Blaster:
                Debug.Log("Blaster PowerUp Collected");
                activeWeapon.SetType(pu.type); // set the active weapon of Hero to Blaster, calling Weapon function
                _activeCooldownBox2 = Instantiate<GameObject>(coolDownBox); // instantiate CoolDownBox from prefab
                _activeCooldownBox2.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false); // move it under canvas so it displays properly
                _activeCooldownBox2.transform.Translate(0, 40, 0); // moves the cooldown UI up so it doesn't overlap the invincible one, and can stack
                _activeCooldownBox2.GetComponent<CooldownManager>().activateCooldown(pu.type); // get the script of the cooldown and activate it, passing powerup type 
                _countingdownBox2 = true; // begin tracking cooldown for deactivaton of powerup
                break;
        }
        pu.AbsorbedBy(this.gameObject);
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