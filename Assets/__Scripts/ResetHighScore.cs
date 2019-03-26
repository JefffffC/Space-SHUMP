using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetHighScore : MonoBehaviour
{

    public Button resetBtn;

    // Start is called before the first frame update
    void Start()
    {
        resetBtn.onClick.AddListener(resetHighscore);
    }

    public void resetHighscore()
    {
        Debug.Log("Attempted high score reset");
        ScoreManager.SM.resetHighscore();
    }
}
