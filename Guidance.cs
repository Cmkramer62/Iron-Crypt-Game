using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Guidance : MonoBehaviour{

    public bool textPopUp = true;
    public bool strongTextPopUp = false;
    public bool objectivePopUp = false;
    public bool damaging = false;

    public bool timed = false;
    private bool inside = false;
    public float timer;

    public bool oneAndDone = false;
    private bool done = false;

    public float cooldown = 6f;
    private bool isReady = true;

    #region TEXT VARS
    public string popUpText = "What to do...";
    public string objectiveText = "Investigate the building.";
    public float duration = 4f;
    #endregion

    #region VISUAL VARS
    private CanvasGroup BloodOverlay;
    private bool stopOverlay = false;
    #endregion

    #region SOUND VARS
    private GameObject helperSystem;
    #endregion


    private void Update(){
        if (damaging && stopOverlay) {
            BloodOverlay.alpha -= 1 * Time.deltaTime;
            if(BloodOverlay.alpha == 0){
                stopOverlay = false;
            }
        }
        if(timed && inside) {
            timer -= 1 * Time.deltaTime;
            if(timer <= 0) {
                ActivateHint();
                oneAndDone = true;
                done = true;
                timed = false;
            }
        }

    }

    // 1.text pop up 2.on screen pop up (blood overlay?) 3. sound queue
    void OnTriggerEnter(Collider other){
        if (!timed && other.CompareTag("Player") && ((oneAndDone && !done) || !oneAndDone)) {
            if (oneAndDone) done = true;
            ActivateHint();
        } else if (timed && other.CompareTag("Player")) {
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(timed && other.CompareTag("Player")) {
            inside = false;
        }
    }

    public void ActivateHint() {
        if (isReady) {
            isReady = false;
            StartCoroutine(CooldownTimer());
            if (textPopUp) HelperText.PopupMessage(popUpText, duration);
            else if (strongTextPopUp) StrongHelperText.PopupMessage(popUpText, duration);

            if (objectivePopUp) ObjectiveShower.ShowTextTimed(objectiveText, duration);
            if (damaging)
            {
                GameObject.Find("Player Two").GetComponent<HealthSystem>().TakeDamage();
            }
        }
        
    }

    IEnumerator CooldownTimer(){
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

}
