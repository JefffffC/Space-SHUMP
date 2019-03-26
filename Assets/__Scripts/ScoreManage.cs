using UnityEngine;
using UnityEngine.UI;

public class ScoreManage : MonoBehaviour
{
    public static ScoreManage S = null;
    public static int HIGHSCORE = 0;
    public int score = 0;
    private Text _scoreText; 
    private Text _highScoreText;

    void Start()
    {
        if (S == null) S = this; //init singlton
        _scoreText = GetComponent<Text>();
        _highScoreText = transform.GetChild(0).GetComponent<Text>();
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SetHighScore(PlayerPrefs.GetInt("HighScore")); //Gets previous high score
        }
    }

    public void ResetScore()
    {
        UpdateScore(-1 * S.score); //resets the score
    }

    public void UpdateScore(int val)
    {
        S.score += val;
        _scoreText.text = "Score: " + score; //updates score
        if(S.score > HIGHSCORE) //updates highscore if score exceeds previous highscore
        {
            SetHighScore(S.score); //Sets the highscore
            PlayerPrefs.SetInt("HighScore", HIGHSCORE); //saves highscore
        }
    }

    private void SetHighScore(int val)
    {
        HIGHSCORE = val;
        _highScoreText.text = "High Score: " + HIGHSCORE;
    }

}
