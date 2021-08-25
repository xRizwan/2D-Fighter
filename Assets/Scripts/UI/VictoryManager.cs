using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    public Slider player1;
    public Slider player2;
    public GameObject victoryText;
    public GameObject restartButton;
    private Text winnerText;

    void Start()
    {
        winnerText = victoryText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.value <= 0) {
            winnerText.text = "Player 2 is the winner";
            SetActive();
        } else if (player2.value <= 0) {
            winnerText.text = "Player 1 is the winner";
            SetActive();
        }
    }

    void SetActive()
    {
        victoryText.SetActive(true);
        restartButton.SetActive(true);
    }
}
