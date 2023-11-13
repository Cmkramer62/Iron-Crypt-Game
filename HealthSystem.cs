using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour {

    public int lives = 3;
    public GameObject HelperSystems;
    public CanvasGroup BloodOverlay;
    private bool stopOverlay = false;

    private void Update() {
        if (stopOverlay) {
            BloodOverlay.alpha -= 1 * Time.deltaTime;
            if (BloodOverlay.alpha == 0) {
                stopOverlay = false;
            }
        }
    }


    public void RaiseLife() {
        if (lives < 3) {
            lives++;
        } //else do nothing. No going over max.
    }

    public void LowerLife() {
        lives--;
        if (lives <= 0) {
            // Player dies.
            HelperSystems.GetComponent<DeathReset>().DeathSequenceStart(0);
        }
    }

    public void TakeDamage() {
        LowerLife();
        HelperSystems.GetComponent<PainSound>().playRandomPainSound();
        BloodOverlay.alpha = 1;
        StartCoroutine(BloodCooldown());
    }
    private IEnumerator BloodCooldown() {
        yield return new WaitForSeconds(6);
        stopOverlay = true;
    }
}
