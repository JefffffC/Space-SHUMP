using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Set in Inspector: Enemy")]
    protected float speed = 5f; //speed in m/s
    public float fireRate = 0.3f; //seconds/shot (to be implemented later)
    public float health = 10;
    public int score = 100; //points earned for destroying enemy

    public float showDamageDuration = 0.05f; // # of seconds to show damage indicator
    public float powerUpDropChance = 0.1f; // likelihood of PowerUp spawn

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials; // all materials of this and its children
    public bool showingDamage = false;
    public float damageDoneTime; // time to stop showing damage
    public bool notifiedofDestruction = false; // will be used later

    protected BoundsCheck bndCheck;
    private bool _destroyedFlag = false;

    protected AudioSource hitSound; //sound for when enemy has been hit and destroyed

    //property: method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Awake()
    {
        hitSound = GetComponent<AudioSource>(); //initializing the sound

        this.bndCheck = GetComponent<BoundsCheck>();
        // get materials and colors for this GameObject and its children
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    void Update()
    {
        Move();
        if (showingDamage && Time.time > damageDoneTime) // once time surpasses the amount of time for showing dmg, it unshows the dmg
        {
            UnShowDamage();
        }
        if (this.bndCheck != null && this.bndCheck.offDown || this.bndCheck.offLeft || this.bndCheck.offRight)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void Move() // this is overriden by each of the enemy children
    {
  
    }

    void OnCollisionEnter (Collision coll) 
    {
        hitSound.Play(); //play sound of enemy being hit on collision
        GameObject otherGO = coll.gameObject; // get the GameObject off the collider that was hit in the collision
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Debug.Log("Enemy Projectile Entrance Detected"); // DEBUG MESSAGE
                Projectile p = otherGO.GetComponent<Projectile>();
                // if this enemy is off screen, don't damage it
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                ShowDamage();
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0) // destroy enemy if health depleted
                {
                    if (!notifiedofDestruction)
                    {
                        Main.S.EnemyDestroyed(this); // pass the reference to the destroyed enemy to the EnemyDestroyed method of Main
                    }
                    notifiedofDestruction = true; // bool which enforces only one pass to Main singleton, ensuring only one PowerUp may spawn
                    Destroy(this.gameObject);
                    while (_destroyedFlag == false) // this loop is a solution to an issue of double-counting the score
                    {
                        ScoreManager.SM.updateCurrScore(score); // add to score
                        _destroyedFlag = true; // once an object has been score-updated, it cannot be score-updated again.
                    }
                    Debug.Log("Enemy destroyed: " + score); // display debugging score
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }

    void ShowDamage() // for showing damage (turning red)
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}