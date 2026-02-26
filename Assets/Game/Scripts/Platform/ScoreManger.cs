using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int starsCollected = 0;
    public int score = 0;

    public Text gamescoreText;

    public Text resultscoreText;
    public Text highscoreText;
    public Text starcountText;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!GameManager.IsGameStart)
        {
            return;
        }

        score = Mathf.FloorToInt(PlayerController2d.Instance.transform.position.y);
        gamescoreText.text = "" + score;
    }

    public void AddStar()
    {
        starsCollected++;
        Debug.Log("Stars: " + starsCollected);
        starcountText.text = "" + starsCollected;
    }

    public void ResetScore()
    {
        starsCollected = 0;
        score = 0;
        gamescoreText.text = "" + score;
        starcountText.text = "" + starsCollected;
    }
    
    public void HighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            Debug.Log("New High Score: " + score);
        }
        highscoreText.text = "" + PlayerPrefs.GetInt("HighScore", 0);
        resultscoreText.text = "" + score;
    }
}