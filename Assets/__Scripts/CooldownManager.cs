using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class CooldownManager : MonoBehaviour
{
    static public CooldownManager CM;

    [Header("Set in the Inspector")]
    public GameObject powerupCooldownUI;
    public bool coolingDownInvincible = false; // coolingDown true shrinks the coolDown bar
    public bool coolingDownRandomWeapon = false;
    public float cooldownInvincibleTime = 10.0f; // cooldown time of Invincible powerup
    public float cooldownRandomWeaponTime = 30.0f; // cooldown time of RandomWeapon powerup

    [Header("Set Dynamically")]
    private Color _powerupColor; // powerup color is determined from weapon type passed by Hero
    private GameObject _activeCooldownBox1;
    private GameObject _activeCooldownBox2;
    private Image _cooldownBar1, _cooldownBar2;
    private Text _cooldownLabel1, _cooldownLabel2;
    private WeaponType _type;

    void Awake()
    {
        CM = this;
        Debug.Log("CooldownUI Manager Activated");
    }

    public void activateCooldown (WeaponType wt)
    {
        _type = wt;
        switch (wt) // switch which assigns cooldowns and text depending on type of powerup passed to function
        {
            case WeaponType.Invincible:
                if (coolingDownInvincible == false) // if Invincible is not active, then instantiate a new one
                {
                    // ACTIVATE INVINCIBILITY HERE
                    _activeCooldownBox1 = Instantiate<GameObject>(powerupCooldownUI); // instantiate CoolDownBox from prefab
                    _activeCooldownBox1.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false); // move it under canvas so it displays properly
                    Debug.Log("Attempted instantiation of invincible box");
                    coolingDownInvincible = true;
                    _cooldownLabel1 = _activeCooldownBox1.gameObject.transform.GetChild(0).GetComponent<Text>(); // obtain reference variable to Text child of prefab
                    _cooldownBar1 = _activeCooldownBox1.gameObject.transform.GetChild(1).GetComponent<Image>(); // obtain reference variable to Image child of prefab
                    WeaponDefinition def = Main.GetWeaponDefinition(_type); // get characteristics of powerup using static function from Main
                    _powerupColor = def.color; // set colors according to powerup type
                    _cooldownBar1.color = _powerupColor;
                    _cooldownLabel1.color = _powerupColor;
                    _cooldownLabel1.text = "Invincibility";
                }
                else
                {
                    _cooldownBar1.fillAmount = 1; // reset bar
                }
                break;
            case WeaponType.Blaster:
                if (coolingDownRandomWeapon == false) // if Weapon is not active, then instantiate a new one
                {
                    Hero.S.activeWeapon.SetType(wt); // set Hero's weapon type to blaster
                    _activeCooldownBox2 = Instantiate<GameObject>(powerupCooldownUI); // instantiate CoolDownBox from prefab
                    _activeCooldownBox2.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false); // move it under canvas so it displays properly
                    _activeCooldownBox2.transform.Translate(0, 40, 0); // moves the cooldown UI up so it doesn't overlap the invincible one, and can stack
                    Debug.Log("Attempted instantiation of weapon box");
                    coolingDownRandomWeapon = true;
                    _cooldownLabel2 = _activeCooldownBox2.gameObject.transform.GetChild(0).GetComponent<Text>(); // obtain reference variable to Text child of prefab
                    _cooldownBar2 = _activeCooldownBox2.gameObject.transform.GetChild(1).GetComponent<Image>(); // obtain reference variable to Image child of prefab
                    WeaponDefinition def = Main.GetWeaponDefinition(_type); // get characteristics of powerup using static function from Main
                    _powerupColor = def.color; // set colors according to powerup type
                    _cooldownBar2.color = _powerupColor;
                    _cooldownLabel2.color = _powerupColor;
                    _cooldownLabel2.text = "Triple Blaster";
                }
                else
                {
                    _cooldownBar2.fillAmount = 1; // reset bar
                }
                break;
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        if (coolingDownInvincible == true)
        {
            _cooldownBar1.fillAmount -= 1.0f / cooldownInvincibleTime * Time.deltaTime;
        }
        if (coolingDownRandomWeapon == true)
        {
            _cooldownBar2.fillAmount -= 1.0f / cooldownRandomWeaponTime * Time.deltaTime;
        }
        if (_cooldownBar1 != null) // enclosing in null check ensures no null reference error if it hasnt been instantiated
        {
            if (_cooldownBar1.fillAmount == 0)
            {
                coolingDownInvincible = false; // stop decrementing timer
                // DEACTIVATE INVINCIBILITY HERE
                Destroy(_activeCooldownBox1); // destroy cooldownUI once expired
            }
        }
        if (_cooldownBar2 != null) // enclosing in null check ensures no null reference error if it hasnt been instantiated
        {
            if (_cooldownBar2.fillAmount == 0)
            {
                coolingDownRandomWeapon = false; // stop decrementing timer
                Hero.S.activeWeapon.SetType(Hero.S.activeWeapon.defaultWeapon); // restore to simple weapon upon expiry
                Destroy(_activeCooldownBox2); // destroy cooldownUI once expired
            }
        }
    }
}