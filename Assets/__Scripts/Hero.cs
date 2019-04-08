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
    public WeaponType defaultWeapon = WeaponType.Simple;
    public bool isInvincible = false; // toggle for invincibility (for powerup)
    public Material InvincibleMaterial;
    public int extraLives = 3;
    public GameObject respawnScreen;


    [Header("Set Dynamically")]
    [SerializeField] // forces Unity to still show _shieldLevel despite being private
    private float _shieldLevel = 1;
    private bool _invincibleChangeFlag = false;

    public delegate void WeaponFireDelegate(); // declare a new delegate type WeaponFireDelegate
    public WeaponFireDelegate fireDelegate;
    public Material originalWingMat, originalBodyMat;

    private AudioSource shieldDownSound; //sound for when shields are down
    private GameObject _respawnScreen;

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
        Renderer heroWing = S.transform.Find("Wing").GetComponent<Renderer>(); // get the renderer of the Wing child of Hero
        Renderer heroBody = S.transform.Find("Cockpit").GetComponentInChildren<Renderer>(); // get the renderer of the Body grandchild of Hero

        originalWingMat = heroWing.material; // get the material of the original 
        originalBodyMat = heroBody.material; // get the material of the original

        shieldDownSound = GetComponent<AudioSource>(); //initializing the sound

        for (int i = 0; i < extraLives; i++) // loop which uses ELM singleton to generate prefabs representing starting extra lives on boot up
        {
            ExtraLifeManager.ELM.addExtraLife();
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
        if ((isInvincible == true) && (_invincibleChangeFlag == false)) // event listener which toggles invincible look based on public bool
        {
            S._goInvincible(true);
            _invincibleChangeFlag = true;
        }
        if ((isInvincible == false) && (_invincibleChangeFlag == true)) // event listner which toggles it back to normal based on public bool
        {
            S._goInvincible(false);
            _invincibleChangeFlag = false;
        }

    }

    private void _goInvincible(bool toggle)
    {
        if (toggle == true)
        {
            Renderer heroWing = S.transform.Find("Wing").GetComponent<Renderer>(); // get the renderer of the Wing child of Hero
            Renderer heroBody = S.transform.Find("Cockpit").GetComponentInChildren<Renderer>(); // get the renderer of the Body grandchild of Hero
            heroBody.material = InvincibleMaterial; // set to custom invincible material to let the player know (and is pretty)
            heroWing.material = InvincibleMaterial;
        }
        else
        {
            Renderer heroWing = S.transform.Find("Wing").GetComponent<Renderer>(); // get the renderer of the Wing child of Hero
            Renderer heroBody = S.transform.Find("Cockpit").GetComponentInChildren<Renderer>(); // get the renderer of the Body grandchild of Hero
            heroWing.material = originalWingMat; // restore original materials
            heroBody.material = originalBodyMat;
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
            if (S.isInvincible == false) // only deplete shields if player is not invincible
            {
                --shieldLevel;
                shieldDownSound.Play(); //play sound of shield destroyed
            }
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
                ExtraLifeManager.ELM.addExtraLife(); // activate corresponding function of singleton Extra Life Manager
                ++extraLives; // increment extra lives count
                Debug.Log("BonusLife PowerUp collected");
                break;
            case WeaponType.Invincible:
                CooldownManager.CM.activateCooldown(pu.type); // activate corresponding function of singleton Cooldown Manager, passing powerup type
                Debug.Log("Invincible PowerUp Collected");
                break;
            case WeaponType.Blaster:
                CooldownManager.CM.activateCooldown(pu.type); // activate corresponding function of singleton Cooldown Manager, passing powerup type
                Debug.Log("Blaster PowerUp Collected");
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
                this.gameObject.SetActive(false); // DEACTIVATE the ship, and don't immediately end the game
                --extraLives; // decrement extra lives
                ExtraLifeManager.ELM.removeExtraLife(); // remove extra life from side bar (Extra Life Manager accounts for negative life balance)
                if (extraLives < 0) // running out of extra lives and then dying means game over, full restart
                {
                    Destroy(this.gameObject); // destroy the ship and restart the game
                    Main.S.DelayedRestart(gameRestartDelay);
                }
                else // if extraLives >= 0, the game still continues - respawn Hero, and grant invincibility, renew shields
                {
                    _respawnScreen = Instantiate<GameObject>(respawnScreen); // instantiate game over screen
                    _respawnScreen.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false); // set parent to Canvas so game over is visible
                    delayedRespawn();
                }
            }
        }

    }
    public void delayedRespawn()
    {
        Invoke("respawn", gameRestartDelay -1f);
    }
    public void respawn()
    {
        Destroy(_respawnScreen);
        S.gameObject.SetActive(true); // "respawn" hero and reposition accordingly
        S.gameObject.transform.position = new Vector3(0, 0, 0);
        S._shieldLevel = 1; // restore shields to max
        WeaponType wt = WeaponType.Invincible; // grant temporary invincibility to make it more fair
        CooldownManager.CM.activateCooldown(wt); 
    }

}