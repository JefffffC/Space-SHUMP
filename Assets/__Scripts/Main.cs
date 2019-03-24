using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Main : MonoBehaviour
{

    static public Main S; // singleton for Main
    private float _tm = 0.0f;

    public float spawnEverySec = 2.0f; // rate of enemy spawns;
    public GameObject[] prefabEnemies; // array of prefab enemies


    void Awake()
    {
        S = this; // in order to set S to refer to specific instance of Main object, do it at the beginning
    }

    // Update is called once per frame
    void Update()
    {
        _tm += Time.deltaTime;
        if (_tm >= spawnEverySec)
        {
            _tm = _tm - spawnEverySec;
            GameObject Enemy;
            int c = (int)Random.Range(4, 8);
            if (c == 4)
            {
                Enemy = Instantiate<GameObject>(prefabEnemies[0]);
            }
            else if (c == 5)
            {
                Enemy = Instantiate<GameObject>(prefabEnemies[1]);
            }
            else
            {
                Enemy = Instantiate<GameObject>(prefabEnemies[2]);
            }
            float xPos = Random.Range(-30, 30);
            Enemy.transform.position = new Vector3(xPos, 45f);
        }
    }

    public void DelayedRestart(float delay)
    {
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}