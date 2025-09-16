using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public float roundTime = 30f;
    private float timeLeft;
    private bool isRunning = false;

    public TextMeshProUGUI timerText;
    public GameObject endScreen;

    public PopupText popupText;

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            EndRound();
        }
    }

    public void StartRound()
    {
        timeLeft = roundTime;
        isRunning = true;
        GameManager.Instance.ResetScore();

        endScreen.SetActive(false);
    }

    private void EndRound()
    {
        isRunning = false;
       // Time.timeScale = 0f; // pause game

        // Update score + check highscore
        GameManager.Instance.SetScore(PokeManager.Instance.GetScore());

        endScreen.SetActive(true);
    }

    // UI Button Functions
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // replace with your main menu scene name
    }

    // Called by other scripts when player scores
    public bool CanScore()
    {
        return isRunning;
    }
}
