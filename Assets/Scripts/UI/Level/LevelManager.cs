using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject restartButton;
    public GameObject mainScreenButton;
    public GameObject resultText;

    public string winString = "Congratulations, You Won!";
    public string loseString = "You Lost";

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
        // resultText.GetComponent<Text>().text = winString;
        // mainScreenButton.SetActive(true);
        resultText.GetComponent<Text>().text = "Victory!";
        GameOver();
        StartCoroutine("LoadNextLevel");
    }

    void Lose()
    {
        resultText.GetComponent<Text>().text = loseString;
        restartButton.SetActive(true);
        mainScreenButton.SetActive(true);
        GameOver();
    }

    void GameOver()
    {
        isGameEnd = true;
        resultText.SetActive(true);
        if (enemy) {
            if (enemy.GetComponent<BossAI>())
                enemy.GetComponent<BossAI>().enabled = false;
            if (enemy.GetComponent<HumanAI>())
                enemy.GetComponent<HumanAI>().enabled = false;
        }
    }

    private IEnumerator LoadNextLevel()
    {
        if (StoryManager.Instance != null) StoryManager.Instance.hasDefeatedStorm = true;
        yield return new WaitForSeconds(2.5f);
        LevelLoader.Instance.LoadLevel("Country");
    }
}
