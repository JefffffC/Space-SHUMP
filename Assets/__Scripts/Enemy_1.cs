using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    //variables such as points awarded, enemy HP and droprates are set in inspector, from extended Enemy class


    private int _leftorright;
    void Start()
    {
        _leftorright = (int)Random.Range(0, 2);

    }
    public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;

        if (_leftorright == 0)
        {
            tempPos.x -= speed * Time.deltaTime;
        }
        if (_leftorright == 1)
        {
            tempPos.x += speed * Time.deltaTime;
        }

        pos = tempPos;
    }

}