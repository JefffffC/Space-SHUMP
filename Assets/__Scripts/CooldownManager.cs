using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class CooldownManager : MonoBehaviour
{
    [Header("Set in the Inspector")]

    public bool coolingDown = false; // coolingDown true shrinks the coolDown bar
    public float cooldownInvincible = 10.0f; // cooldown time of Invincible powerup
    public float cooldownRandomWeapon = 30.0f; // cooldown time of RandomWeapon powerup
    public Image coolDownBar; // reference to Image GameObject of Prefab, set in Inspector
    public Text powerupLabel; // reference to Text GameObject of Prefab, set in Inspector

    [Header("Set Dynamically")]
    [SerializeField] private Color powerupColor; // powerup color is determined from weapon type passed by Hero
    [SerializeField] private WeaponType type;

    private float cooldownDuration;

    public void activateCooldown (WeaponType wt)
    {
        type = wt;
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        powerupColor = def.color; // get color for CoolDownUI based on powerup type
        coolDownBar.color = powerupColor; // assign the powerup color to the CoolDownUI elements
        powerupLabel.color = powerupColor;
        coolingDown = true; // sets cooling down to be true to engage cooldownBar shrinking
        switch (type) // switch which assigns cooldowns and text depending on type of powerup passed to function
        {
            case WeaponType.Invincible:
                cooldownDuration = cooldownInvincible;
                powerupLabel.text = "Invincibility";
                break;
            case WeaponType.Blaster:
                cooldownDuration = cooldownRandomWeapon;
                powerupLabel.text = "Blaster Gun";
                break;
        }
    }

    void Awake()
    {
        Debug.Log("CooldownUI Manager Activated");
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown == true)
        {
            coolDownBar.fillAmount -= 1.0f / cooldownInvincible * Time.deltaTime;
        }
        if (coolDownBar.fillAmount == 0)
        {
            coolingDown = false;
            Destroy(this.gameObject);
        }
    }
}