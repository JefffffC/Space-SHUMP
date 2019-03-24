using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Set in Inspector: Enemy")]
    protected float speed = 5f; //speed in m/s
    public float fireRate = 0.3f; //seconds/shot
    public float health = 10;
    public int score = 100; //points earned for destroying enemy

    private BoundsCheck bndCheck;

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
        this.bndCheck = GetComponent<BoundsCheck>();
    }

    void Update()
    {
        Move();
        if (this.bndCheck != null && this.bndCheck.offDown || this.bndCheck.offLeft || this.bndCheck.offRight)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}