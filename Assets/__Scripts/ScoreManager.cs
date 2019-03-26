using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager SM;
    public Text highScore;
    public Text currentScore;
    private float _highS;
    private float _currS;

    void Awake()
    {
        _highS = (float)PlayerPrefs.GetInt("highscore", 0);
        highScore.text = "HighScore: " + _highS;
        _currS = 0;
        SM = this;
    }

    public void updateCurrScore(float score)
    {
        _currS += score;
        currentScore.text = "Current Score: " + _currS;
    }

    public void updateHighScore()
    {
        Debug.Log("Attempting HighScore update: " + _currS);
        if (_highS <= _currS)
        {
            _highS = _currS;
        }
        _currS = 0;

        PlayerPrefs.SetInt("highscore", (int)_highS);
        PlayerPrefs.Save();

        highScore.text = "HighScore: " + _highS;
    }
    public void resetHighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        _highS = 0f;
        highScore.text = "HighScore: " + _highS;
    }
}