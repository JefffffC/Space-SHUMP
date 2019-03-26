using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject poi; //player ship
    public GameObject[] panels; //scrolling panels
    public float speed = -3.0f; //scolling speed

    public float motionMult = 0.25f; //motion controls

    private float _paneHeight;
    private float _depth;

    // Start is called before the first frame update
    void Start()
    {
        _paneHeight = panels[0].transform.localScale.y;
        _depth = panels[0].transform.position.y;

        //sets initial panels
        panels[0].transform.position = new Vector3(0, 0, _depth);
        panels[1].transform.position = new Vector3(0, _paneHeight, _depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * speed % _paneHeight + (_paneHeight * 0.5f);

        if(poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
        }

        panels[0].transform.position = new Vector3(tX, tY, _depth); //moves the first pane

        //makes a continuos flow
        if(tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - _paneHeight, _depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + _paneHeight, _depth);
        }
    }
}
