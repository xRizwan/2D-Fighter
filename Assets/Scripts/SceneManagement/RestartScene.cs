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
        if (GameManager.Instance)
            GameManager.Instance.restarted = true;
    }

    public void RestartGame()
    {
        levelLoader.LoadLevel("Menu");
        if (GameManager.Instance)
            GameManager.Instance.restarted = true;
    }
}
