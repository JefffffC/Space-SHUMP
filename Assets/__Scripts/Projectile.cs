using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField] // forces _type to be visible and settable in Unity Inspector although private
    private WeaponType _type; // private field accessed through property using correct naming convention

    public WeaponType type // property
    {
        get
        {
            return _type;
        }

        set
        {
            SetType(value); // lets us do more than just set type
        }
    }

    void Awake ()
    {
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_bndCheck.offUp || _bndCheck.offLeft || _bndCheck.offRight) // if projectile goes off the top of the screen, destroy it
        {
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType etype)
    {
        _type = etype;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }
}
