using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class CooldownManager : MonoBehaviour
{
    [Header("Set in the Inspector")]
    public GameObject coolDownBox;
    public bool coolingDown = false;
    public float cooldownInvincible = 10.0f;
    public float cooldownRandomWeapon = 30.0f;

    [Header("Set Dynamically")]
    [SerializeField] private WeaponType wt;
    [SerializeField] private Color powerupColor;
    [SerializeField] private Image coolDownBar;
    [SerializeField] private Text powerupLabel;

    // Update is called once per frame
    void Update()
    {
        if (coolingDown == true)
        {
            //Reduce fill amount over 30 seconds
            .fillAmount -= 1.0f / cooldownInvincible * Time.deltaTime;
        }
    }
}