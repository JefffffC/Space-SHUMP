using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifeManager : MonoBehaviour
{
    static public ExtraLifeManager ELM;

    [Header("Set in the Inspector")]
    public GameObject ExtraLifeIcon; // prefab which will be instantiated
    public float iconGaps = 45; // vertical distance between life icons
    public int maxExtraLives = 10; // maximum possible number of extra lives, collecting more won't do anything

    [Header("Set Dynamically")]
    private int _numExtraLives = 0; // internal count of number of extra lives
    private Stack<ExtraLife> ExtraLives; // internal stack for LIFO structure of life addition/removal

    private AudioSource respawnSound;

    void Awake()
    {
        ELM = this; // at the very start, set singleton to reference this instance
        Debug.Log("Extra Life Manager Activated"); // debug msg
        ExtraLives = new Stack<ExtraLife>(); // initiate internal stack
        respawnSound = GetComponent<AudioSource>();
    }

    public void addExtraLife()
    {
        if (_numExtraLives == maxExtraLives)
        {
            Debug.Log("Maximum number of lives reached");
            return;
        }
        ExtraLife newLife = new ExtraLife(); // create new ExtraLife item for stack
        newLife.pos = (_numExtraLives * iconGaps); // assign position relative to how many lives are on stack currently
        newLife.ExtraLifeIcon = Instantiate<GameObject>(ExtraLifeIcon); // instantiate prefab
        newLife.ExtraLifeIcon.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false); // move it under canvas so it displays properly
        newLife.ExtraLifeIcon.transform.Translate(0, newLife.pos, 0); // translate prefab to correct position
        ExtraLives.Push(newLife); // push onto stack
        _numExtraLives++; // increment number for correct visual stacking on UI through relative positioning
        Debug.Log("Attempted to add extra life");
    }

    public void removeExtraLife()
    {
        if (_numExtraLives == 0) // avoid empty stack error
        {
            Debug.Log("Already have zero lives!");
            return;
        }
        ExtraLife removedLife = ExtraLives.Pop(); // remove the top-most ExtraLife on the stack
        respawnSound.Play();
        Destroy(removedLife.ExtraLifeIcon); // destroy the GameObject pertaining to the ExtraLife 
        removedLife.ExtraLifeIcon = null; // set previous members to null
        _numExtraLives--; // decrement number for correct visual stacking on UI
    }
}

public class ExtraLife // more convenient, encapsulation of ExtraLife unit
{
    public float pos;
    public GameObject ExtraLifeIcon;

    public ExtraLife()
    {
        pos = 0;
        ExtraLifeIcon = null;
    }
}
