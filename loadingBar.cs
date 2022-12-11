using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadingBar : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progresstext;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1.0f;

        }
        else
        {
            Time.timeScale = 1f;
        }

    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progresstext.text = progress * 100f + "%";
            yield return null;

        }
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
