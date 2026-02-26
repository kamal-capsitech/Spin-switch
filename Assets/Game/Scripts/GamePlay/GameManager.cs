using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool IsGameStart = false;
    public static bool IsGameOver = false;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsGameStart = false;
        Time.timeScale = 1f;
    }

   

    public void StartGame()
    {
        IsGameStart = true;
        
        Time.timeScale = 1f;
    }

    public GameObject resultPanel;
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        IsGameStart= false;
       

        Debug.Log("Game Over");

        // Optional: slow motion effect
        Time.timeScale = 0.5f;
       StartCoroutine(ShowResult());
    }
    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1f);
        resultPanel.SetActive(true);
    }

    public bool isRetrying = false;
    public void RetryGame()
    {
        IsGameStart = true; 
        IsGameOver = false;
        isRetrying = true;
        var playerTransform = PlayerController2d.Instance.transform;
        var position = playerTransform.position;
        position.x = 0;
        position.y += 2;

        playerTransform.position = position;
        
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        IsGameOver = false;
        IsGameStart = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}