using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject restartButton;
    public GameObject mainScreenButton;
    public GameObject resultText;

    string loseString = "You Lost!";
    string winString = "You Won!";

    public bool isGameEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (isGameEnd) return;

        if (enemy && enemy.GetComponent<HealthManager>().health <= 0) Win();
        else if(player.GetComponent<HealthManager>().health <= 0) Lose();
    }

    void Win()
    {
        resultText.GetComponent<Text>().text = winString;
        mainScreenButton.SetActive(true);
        GameOver();
    }

    void Lose()
    {
        resultText.GetComponent<Text>().text = loseString;
        restartButton.SetActive(true);
        GameOver();
    }

    void GameOver()
    {
        isGameEnd = true;
        resultText.SetActive(true);
        if (enemy) enemy.GetComponent<BossAI>().enabled = false;
    }
}
