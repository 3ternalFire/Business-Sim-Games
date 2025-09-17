using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        Grid.OnScoreAdded += UpdateScore;
    }

    private void OnDisable()
    {
        Grid.OnScoreAdded -= UpdateScore;
    }
    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.SetText($"Score: {score}");
        }
    }
}
