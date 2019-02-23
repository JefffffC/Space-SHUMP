using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
  
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -29f, 29f),
      Mathf.Clamp(transform.position.y, -39f, 39f), transform.position.z);
    }
}
