using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string waveScene;

    public GameObject loadingScreen;

    public TextMeshProUGUI loadingText;
    public Image progressBar;

    private void Start(){
        AudioManager.instance.PlayNoise("Menu");
    }

    public void Quit(){
        Application.Quit();
    }

    public void Play(){
        StartCoroutine(LoadSceneAsync(waveScene));
    }

    // Coroutine to load the scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.fillAmount = progress;

            loadingText.text = $"Loading... {progress * 100}%";

            if (operation.progress >= 0.9f)
            {
                loadingText.text = "Press any key to continue...";
                progressBar.fillAmount = 1;

                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
