using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadAsynchronously(name));
    }

    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation opertaion = SceneManager.LoadSceneAsync(name);
        loadingScreen.SetActive(true);

        while (!opertaion.isDone) {

            float progress = Mathf.Clamp01(opertaion.progress / 0.9f);
            Debug.Log(progress);
            slider.value = progress;
            yield return null;
        }
    }
}
