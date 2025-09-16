using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int _score { get; private set; }

    public int _highScore { get; private set; }

    public static UnityAction<string, Color> OnHighScoreAchieved;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    public bool CheckForHighScore()
    {
        if(_score > _highScore)
        {
            _highScore = _score;
            OnHighScoreAchieved?.Invoke("HIGH SCORE!", Color.white);
            PlayerPrefs.SetInt("HighScore", _score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public void SetScore(int newScore)
    {
        _score = newScore;
        CheckForHighScore();
    }
    public void ResetScore()
    {
        _score = 0;
    }
}
