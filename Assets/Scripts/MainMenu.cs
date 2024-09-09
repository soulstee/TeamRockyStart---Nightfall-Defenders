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
    public GameObject mainScreen;
    public GameObject[] scenes;

    public TextMeshProUGUI loadingText;
    public Image progressBar;
    private int sceneActivationInt = 0;

    private void Start(){
        AudioManager.instance.PlayNoise("Menu");
    }

    public void Quit(){
        Application.Quit();
    }

    public void Play(){
        StartCoroutine(LoadSceneAsync(waveScene));
    }

    public void PlayButtonNoise(){
        AudioManager.instance.PlayNoise("Button");
    }

    bool loading = false;

    private void Update(){
        if (Input.anyKeyDown && loading)
        {
            sceneActivationInt++;
        }
    }

    // Coroutine to load the scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loading = true;
        bool sceneReady = false;
        loadingScreen.SetActive(true);
        scenes[0].SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if(sceneActivationInt == 1){
                scenes[sceneActivationInt].SetActive(true);
                scenes[sceneActivationInt-1].SetActive(false);
            }else if(sceneActivationInt == 2){
                    sceneReady = true;
            }

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.fillAmount = progress;

            loadingText.text = $"Loading... {progress * 100}%";

            if (operation.progress >= 0.9f)
            {
                loadingText.text = "Press any key to continue...";
                progressBar.fillAmount = 1;
            }

            if (sceneReady){
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
