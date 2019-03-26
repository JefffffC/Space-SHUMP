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

    private float panelHt;
    private float depth;

    void Start()
    {
        panelHt = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;

        //set initial pos of panels
        panels[0].transform.position = new Vector3(0, 0, depth);
        panels[1].transform.position = new Vector3(0, panelHt, depth);

        //set initial pos of BGP (background parallax panels)

        panels[2].transform.position = new Vector3(0, 0, depth);
        panels[3].transform.position = new Vector3(0, panelHt, depth);
    }

    void Update()
    {
        float tY, tYbgp;
        float tX = 0;
        float tXbgp = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);

        tYbgp = Time.time * (scrollSpeed * 0.5f) % panelHt + (panelHt * 0.5f); // slower parallaxing BGP to heighten sense of depth


        if (poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
            tXbgp = -poi.transform.position.x * motionMult * 0.5f; // slower parallaxing BGP to heighten sense of depth
        }

        // position panels[0]
        panels[0].transform.position = new Vector3(tX, tY, depth);
        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - panelHt, depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + panelHt, depth);
        }

        // position bgps
        panels[2].transform.position = new Vector3(tXbgp, tYbgp, depth);
        if (tYbgp >= 0)
        {
            panels[3].transform.position = new Vector3(tX, tYbgp - panelHt, depth);
        }
        else
        {
            panels[3].transform.position = new Vector3(tX, tYbgp + panelHt, depth);
        }
    }
}
