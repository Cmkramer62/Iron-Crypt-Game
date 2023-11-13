using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;

public class DeathReset : MonoBehaviour {
    public GameObject player;
    public Transform softCheckpoint;

    public Animator fadeAnimator;
    public int nextLevel = 0;

    #region HELPER METHODS
    /*
     * Freeze the player and the camera.
     */
    private void FreezeCharacter() {
        PlayerNullify.HardSetDisallow();
    }

    private void FadeToBlack() {
        fadeAnimator.Play("FadeIn3");
        GameObject.Find("Bars Transition").GetComponent<Transition>().PlayBarsDown();
    }

    private void DeathEffect(int effectNumber) {
        if (effectNumber == 0) { // Hard Reset.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else if (effectNumber == 1) {// Soft Reset.
            // do some magic. reset scene while also teleporting player to the shits...what about items?
            //player.transform.position = softCheckpoint.position;
            //player.transform.rotation = softCheckpoint.rotation;
        } else if (effectNumber  == 2) {// Fake Reset.
            player.transform.position = softCheckpoint.position;
            player.transform.rotation = softCheckpoint.rotation;
        } else if (effectNumber == 3) { // Load Level
            SceneManager.LoadScene(nextLevel);
        }
    }

    private void UnfreezeCharacter() {
        PlayerNullify.HardSetAllow();
    }

    private void UnfadeFromBlack() {
        fadeAnimator.Play("FadeOut3");
        GameObject.Find("Bars Transition").GetComponent<Transition>().PlayBarsUp();
    }
    #endregion

    public void DeathSequenceStart(int deathType) {
        StartCoroutine(DeathSequence(deathType));
    }

    private IEnumerator DeathSequence(int deathType) {
        FreezeCharacter();
        FadeToBlack();
        yield return new WaitForSeconds(3f);
        DeathEffect(deathType);
        UnfreezeCharacter();
        UnfadeFromBlack();
    }

}
