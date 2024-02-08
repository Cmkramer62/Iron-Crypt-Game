using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class DeathReset : MonoBehaviour {
    public GameObject player;
    public Transform softCheckpoint;

    public Animator fadeAnimator;
    public int nextLevel = 0;

    public GameObject loadingScreen;
    public Slider slider;

    public AudioSource loadingSource;

    public bool useSimpleNullify = false;

    public bool lowerVolume, raiseVolume = false;
    private float t = 0;
    private float g = 0;

    public void Update() {
        if (lowerVolume) {
            gameObject.GetComponent<GameSettings>().SetVolume(Mathf.Lerp(0, -80, t));
            t += .3f * Time.deltaTime;
        } else if (raiseVolume) {
            gameObject.GetComponent<GameSettings>().SetVolume(Mathf.Lerp(-80, 0, g));
            g += .7f * Time.deltaTime;
            bool result = gameObject.GetComponent<GameSettings>().audioMixer.GetFloat("TestVolume", out float temp);
            if (result && temp >= 0){
                raiseVolume = false;
                g = 0;
            }
        }
    }


    #region HELPER METHODS
    /*
     * Freeze the player and the camera.
     */
    private void FreezeCharacter() {
        if (!useSimpleNullify) PlayerNullify.HardSetDisallow();
        else PlayerNullify.SimpleHardSetDisallow();
    }

    private void FadeToBlack() {
        fadeAnimator.Play("FadeIn3");
        GameObject.Find("Bars Transition").GetComponent<Transition>().PlayBarsDown();
    }

    private void DeathEffect(int effectNumber) {
        if (effectNumber == 0) { // Hard Reset.
            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else if (effectNumber == 1) {// Soft Reset.
            // do some magic. reset scene while also teleporting player to the shits...what about items?
            //player.transform.position = softCheckpoint.position;
            //player.transform.rotation = softCheckpoint.rotation;
        } else if (effectNumber  == 2) {// Fake Reset.
            player.transform.position = softCheckpoint.position;
            player.transform.rotation = softCheckpoint.rotation;
        } else if (effectNumber == 3) { // Load Level
            //SceneManager.LoadScene(nextLevel);
            StartCoroutine(LoadAsynchronously(nextLevel));
        }
    }

    private void UnfreezeCharacter() {
        if (!useSimpleNullify) PlayerNullify.HardSetAllow();
        else PlayerNullify.SimpleHardSetAllow();
    }

    private void UnfadeFromBlack() {
        fadeAnimator.Play("FadeOut3");
        GameObject.Find("Bars Transition").GetComponent<Transition>().PlayBarsUp();
    }
    #endregion

    public void StopLoadingSource() {
        loadingSource.gameObject.SetActive(false);
    }

    public void DeathSequenceStart(int deathType) {
        StartCoroutine(DeathSequence(deathType));
    }

    private IEnumerator DeathSequence(int deathType) {
        FreezeCharacter();
        FadeToBlack();
        lowerVolume = true;
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<GameSettings>().SetVolume(-80);
        loadingSource.gameObject.SetActive(true);
        lowerVolume = false;
        t = 0;
        GameObject.Find("Player Two").transform.position = new Vector3(500, 500, 500);

        GameObject MC = GameObject.Find("Main Camera");
        MC.GetComponent<MouseLook>().allowedToLook = false;
        MC.GetComponent<MouseLook>().xRotation = 0f;

        DeathEffect(deathType);
        UnfreezeCharacter();
        if (deathType == 2) UnfadeFromBlack();
    }

    IEnumerator LoadAsynchronously(int sceneIndex) {
        
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(4f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        
        UnfadeFromBlack();
    }

}
