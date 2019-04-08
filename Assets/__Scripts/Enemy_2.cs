using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    public GameObject target;
    public float movementSpeed = 15f;
    public float rotationSpeed = 90f;

    private float _currentAngle;
    private float _targetAngle;

    // Use this for initialization
    void Start()
    {

        // Force object to be at Z = 0
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;

        _currentAngle = GetAngleToTarget();
    }

    // Update is called once per frame
    public override void Move()
    {

        _targetAngle = GetAngleToTarget();
        _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, _targetAngle, rotationSpeed * Time.deltaTime);
        transform.position += Quaternion.Euler(0, 0, _currentAngle) * Vector3.right * movementSpeed * Time.deltaTime;

    }

    float GetAngleToTarget()
    {

        Vector3 v3 = target.transform.position - transform.position;
        return Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
    }


}