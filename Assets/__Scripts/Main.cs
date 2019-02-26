using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSec = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck bndCheck;
   
    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1.0f / enemySpawnPerSec);

    }

    public void SpawnEnemy()
    {
        //pick random enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);


        //position the enmey above screen with random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //set intial position for spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;

        float[] xpos1 = new float[2];
        xpos1[0] = (int)xMin;
        xpos1[1] = (int)xMax;
        if (ndx == 1)
        {
            float p = Random.Range(0, xpos1.Length);
            if (p == 0)
            {
                pos.x = xpos1[0];
            }
            else
            {
                pos.x = xpos1[1];
            }
        }
        else
        {
            pos.x = Random.Range(xMin, xMax);
        }

            pos.y = bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;

            //invoke spawn enemy again
            Invoke("SpawnEnemy", 1.0f / enemySpawnPerSec);
        
    }
}


    

