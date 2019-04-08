using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set In Inspector")]
    public GameObject poi; // player ship
    public GameObject[] panels; // scrolling foregrounds
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f; // how much parallax based on player motion

    private float _panelHt;
    private float _depth;

    void Start()
    {
        _panelHt = panels[0].transform.localScale.y;
        _depth = panels[0].transform.position.z;

        //set initial pos of panels
        panels[0].transform.position = new Vector3(0, 0, _depth);
        panels[1].transform.position = new Vector3(0, _panelHt, _depth);

        //set initial pos of BGP (background parallax panels)

        panels[2].transform.position = new Vector3(0, 0, _depth);
        panels[3].transform.position = new Vector3(0, _panelHt, _depth);
    }

    void Update()
    {
        float tY, tYbgp;
        float tX = 0;
        float tXbgp = 0;
        tY = Time.time * scrollSpeed % _panelHt + (_panelHt * 0.5f);

        tYbgp = Time.time * (scrollSpeed * 0.5f) % _panelHt + (_panelHt * 0.5f); // slower parallaxing BGP to heighten sense of depth


        if (poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
            tXbgp = -poi.transform.position.x * motionMult * 0.5f; // slower parallaxing BGP to heighten sense of depth
        }

        // position panels[0]
        panels[0].transform.position = new Vector3(tX, tY, _depth);
        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - _panelHt, _depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + _panelHt, _depth);
        }

        // position bgps
        panels[2].transform.position = new Vector3(tXbgp, tYbgp, _depth);
        if (tYbgp >= 0)
        {
            panels[3].transform.position = new Vector3(tX, tYbgp - _panelHt, _depth);
        }
        else
        {
            panels[3].transform.position = new Vector3(tX, tYbgp + _panelHt, _depth);
        }
    }
}
