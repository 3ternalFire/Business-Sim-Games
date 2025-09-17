using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleGame
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _highScoreText;

        private void OnEnable()
        {
            _highScoreText.text = $"High Score: \n 00{PlayerPrefs.GetInt("HighScore", 0)}";
        }
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ResetStats()
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
            _highScoreText.text = $"High Score: \n 00{PlayerPrefs.GetInt("HighScore", 0)}";
        }
    }

}
