using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Main : MonoBehaviour
{

    static public Main S; // singleton for Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    private float _tm = 0.0f;

    public float spawnEverySec = 2.0f; // rate of enemy spawns;
    public GameObject[] prefabEnemies; // array of prefab enemies

    public WeaponDefinition[] weaponDefinitions; // list of weapon types, placed in Main class


    void Awake()
    {
        S = this; // in order to set S to refer to specific instance of Main object, do it at the beginning

        // A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
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
        SceneManager.LoadScene("_Scene_0"); // restarts the game by reloading the scene
        ScoreManager.SM.updateHighScore();
    }

    static public WeaponDefinition GetWeaponDefinition (WeaponType wt) //static function which allows other classes to access the data in WEAP_DICT
    {
        if (WEAP_DICT.ContainsKey(wt)) // check to make sure key exists in the dictionary, which is important to avoid error for non-existent key
        {
            return (WEAP_DICT[wt]);
        }
        return (new WeaponDefinition()); // returns a new WeaponDefinition with type of WeaponType.none, case where it fails to find right definition
    }
}