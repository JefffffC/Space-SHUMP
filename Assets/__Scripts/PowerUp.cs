using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float lifeTime = 6f;
    public float fadeTime = 4f;
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 horizontalVelocityMinMax = new Vector2(0f, 5f);
    public Vector2 verticalVelocityMinMax = new Vector2(5f, 10f);


    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;


    private Rigidbody _rigid;
    private BoundsCheck _bndCheck;
    private Renderer _cubeRend;
    private Vector3 _velocity;
    private float _gravity = 9.81f;

    void Awake()
    {
        // find reference to cube object
        cube = transform.Find("Cube").gameObject;
        // find the text mesh and other components for entire PowerUp
        letter = GetComponent<TextMesh>();
        _rigid = GetComponent<Rigidbody>();
        _bndCheck = GetComponent<BoundsCheck>();
        _cubeRend = cube.GetComponent<Renderer>();
        
        // same as no rotation
        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;

        int horizontalDirection = Random.Range(0, 2);
        if (horizontalDirection == 0)
        {
            _velocity = new Vector3(Random.Range(horizontalVelocityMinMax.x, horizontalVelocityMinMax.y),
                Random.Range(verticalVelocityMinMax.x, verticalVelocityMinMax.y), 0f);
        }
        else
        {
            _velocity = new Vector3(-1f * Random.Range(horizontalVelocityMinMax.x, horizontalVelocityMinMax.y),
                Random.Range(verticalVelocityMinMax.x, verticalVelocityMinMax.y), 0f);
        }
    }

    void Update()
    {
        //manually rotate the cube child every update, based on time
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //apply falling system similar to gravity

        _velocity.y -= _gravity * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;

        //Fade out the PowerUp over time

        float powerUpTime = (Time.time - (birthTime + lifeTime)) / fadeTime;

        if (powerUpTime >= 1) {
            Destroy(this.gameObject);
            return;
        }

        if (powerUpTime > 0) // use powerUpTime to determine alpha of cube and letter
        {
            Color c = _cubeRend.material.color;
            c.a = 1f - powerUpTime;
            _cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - (powerUpTime * 0.5f); // fade out the letter not as much as the cube
            letter.color = c;
        }

        if (!_bndCheck.isOnScreen)
        {
            // if the powerup drifts off the screen, destroy it
            Destroy(gameObject);
        }

    }
    public void SetType(WeaponType wt)
    {
        // grab WeaponDefinition from Main
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        _cubeRend.material.color = def.color; // set to appropriate color based on WeaponDefinition
        if ((def.letter == "L") || (def.letter == "I")) {
            letter.text = def.letter; // set to appropriate non-weapon powerup letter
        }
        else
        {
            letter.text = "W"; // set to "W" if the PowerUp is a random weapon
        }
        type = wt;
    }
    public void AbsorbedBy(GameObject target)
    {
        // function that is called by Hero class whenever a PowerUp is collected
        Destroy(this.gameObject);
    }
}
