using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public LevelLoader levelLoader;
    public void Restart()
    {
        levelLoader.LoadLevel(SceneManager.GetActiveScene().name);
        GameManager.Instance.restarted = true;
    }

    public void RestartGame()
    {
        levelLoader.LoadLevel("Menu");
        GameManager.Instance.restarted = true;
    }
}
