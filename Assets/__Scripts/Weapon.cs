using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType // an enum of the various possible weapon types
{
    Simple, // simple, single straight shot
    Blaster // triple shot, one straight and two diagonal in a v-shaped pattern
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField] // make these vars viewable and editable in Unity despite private
    private WeaponType _type = WeaponType.Simple;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; // last time shot was fired
    private Renderer collarRend;

    void Start ()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type); // call SetType() for default _type of WeaponType.Simple

        if (PROJECTILE_ANCHOR == null) // dynamically create an anchor for all projectiles
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        // find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    public void SetType (WeaponType wt)
    {
        _type = wt;
        this.gameObject.SetActive(true);
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color; // set color of collar to the weapon collar definition
        lastShotTime = 0; // can fire immediately after _type is set
    }

    public void Fire()
    {
        // if this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShotTime < def.delayBetweenShots) return;
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity; // move projectile according to velocity
        if (transform.up.y < 0)
        {
            vel.y = -vel.y; // for downward facing weapons if implemented
        }
        switch (type)
        {
            case WeaponType.Simple:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.Blaster:
                p = MakeProjectile(); // make middle projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); // make right projectile
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); // make left projectile
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero") // if the parent is the Hero, then assign Hero corresponding tags to projectile
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero"); 
        }
        else // if parent isnt hero, then it must be enemy (for further enemy gun implementation)
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position; // ensure projectiles fire from collar
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time; // ensure correct firing rate
        return (p);
    }
}

[System.Serializable] // allows the class defined to be serializable and editable within Unity inspector
public class WeaponDefinition
{
    public WeaponType type = WeaponType.Simple; // default weapon
    public GameObject projectilePrefab; // prefab for projectiles
    public Color color = Color.white; // color of collar
    public Color projectileColor = Color.white; // color of projectiles
    public float damageOnHit = 1f; // damage of one projectile hitting
    public float delayBetweenShots = 0.1f; // firing rate of weapon
    public float velocity = 20f; // speed of projectiles from weapon
}
