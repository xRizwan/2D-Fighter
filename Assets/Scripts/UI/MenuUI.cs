using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Classic");
    }

    public void StartLocalMultipler()
    {
        SceneManager.LoadScene("PvP");
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
