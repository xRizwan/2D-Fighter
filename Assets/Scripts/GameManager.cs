using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score;
    public bool restarted;
    public Text scoreText;

    void Start()
    {
        if (Instance != null) {
            Instance.scoreText = scoreText;
            DisplayScore();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Instance.score = score;
        DontDestroyOnLoad(gameObject);

        DisplayScore();
    }

    void Update()
    {
        if (restarted) {score = 0; restarted = false; DisplayScore();}
    }

    public void UpdateScore(int _score)
    {
        Instance.score += _score;
        if (Instance.score < 0) Instance.score = 0;
        DisplayScore();
    }

    public void DisplayScore()
    {
        if (scoreText) Instance.scoreText.text = "Score: " + Instance.score;
    }
}
