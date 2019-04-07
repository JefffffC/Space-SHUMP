using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Main : MonoBehaviour
{

    static public Main S; // singleton for Main
    static public Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    

    public float spawnWait;
    public float startWait;
    public float waveWait; 
    public int enemyCount;
    public Text waveLabel;
    public GameObject[] nextWaveLabels;
    public bool gameOver = false; 
    private int _wave  ;

    public GameObject[] prefabEnemies; // array of prefab enemies

    public WeaponDefinition[] weaponDefinitions; // list of weapon types, placed in Main class
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    { WeaponType.Blaster, WeaponType.Blaster, WeaponType.BonusLife, WeaponType.Invincible }; // list of potential PowerUps, Blaster is more likely

    private AudioSource introSound; //sound for when shields are down


    public void EnemyDestroyed (Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            // choose which PowerUp to pick from possible ones in list
            int randomSelection = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[randomSelection];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
    }

  

    void Awake()
    {
       
        S = this; // in order to set S to refer to specific instance of Main object, do it at the beginning
        StartCoroutine(SpawnWaves());
        _wave = 0;
        // A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

        introSound = GetComponent<AudioSource>(); //initializing the sound
    }

    // Update is called once per frame
    IEnumerator SpawnWaves() // coroutine 
    {
        yield return new WaitForSeconds (startWait);
      
        while (true)
        {
            for (int i=0; i< enemyCount; i++)
            {
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
                else if (c == 6)
                {
                    Enemy = Instantiate<GameObject>(prefabEnemies[2]);
                }
                else if  ((c==7 || c == 8) && _wave > 2)
                {
                    Enemy = Instantiate<GameObject>(prefabEnemies[3]);

                }
                else
                {
                    Enemy = Instantiate<GameObject>(prefabEnemies[1]);
                }
                float xPos = Random.Range(-30, 30);
                Enemy.transform.position = new Vector3(xPos, 45f);
                yield return new WaitForSeconds(spawnWait);
            }
         

          
            _wave++;
            waveLabel.text = "Wave: " + _wave;

            yield return new WaitForSeconds(waveWait);
           
        }
       /* _tm += Time.deltaTime;
        if (_tm >= spawnWait)
        {
            _tm = _tm - startWait;
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
        }*/
      //  yield return new WaitForSeconds (spawnWait); 
    }

    public void DelayedRestart(float delay)
    {
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        introSound.Play(); //play intro sound effect
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