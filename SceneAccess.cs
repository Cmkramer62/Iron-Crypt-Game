using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneAccess : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;

    // Don't rename! Unity doesn't handle renaming for OnClick() Events.
    public void loadScene(int x) {
        StartCoroutine(LoadAsynchronously(x));
    }

    public void ExitGame() {
        Application.Quit();
    }

    private IEnumerator LoadAsynchronously(int sceneIndex) {
        gameObject.GetComponent<GameSettings>().SetVolume(0);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }

}
