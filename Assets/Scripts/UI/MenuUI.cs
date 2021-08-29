using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{

    public LevelLoader levelLoader;

    public void StartGame()
    {
        levelLoader.LoadLevel("Classic");
    }

    public void StartLocalMultipler()
    {
        levelLoader.LoadLevel("PvP");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
