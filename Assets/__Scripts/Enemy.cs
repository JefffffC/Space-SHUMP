using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f; //speed in m/s
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
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Update()
    {
        Move();
       if (bndCheck != null && bndCheck.offDown)
       {
            Destroy(gameObject);
       }
       if (bndCheck != null && !bndCheck.isOnScreen)
       {
            if (pos.y < bndCheck.camHeight - bndCheck.radius)
            {
                Destroy(gameObject);
            }
       }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}


  
   
/*

public class Enemy_0 : Enemy
{
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
public class Enemy_1 : Enemy
{
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
      //  transform.Translate(0, speed * Mathf.Cos(angleRad), speed * Mathf.Sin(angleRad));
        //this one or ^
       // transform.position = Vector3.MoveTowards(transform.position, targPos, speed);
        //this one
    }
}

public class Enemy_2 : Enemy
{
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}*/