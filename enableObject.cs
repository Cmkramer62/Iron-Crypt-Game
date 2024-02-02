using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableObject : MonoBehaviour {
    public GameObject objectToEnable;
    public bool enable = false;

    public bool enterTrigger = true;
    public bool timed = false;
    public float timer = 30f;
    public bool alwaysSetOpposite = false;
    public bool oneAndDone = false;

    private bool inside = false;
    private bool doneTimer = false;

    public bool anythingActivate = false;

    private void Update() {
        if (timed && !doneTimer && inside) {
            timer -= 1 * Time.deltaTime;
            if (timer <= 0) {
                activateManually();
               // oneAndDone = true;
                //done = true;
                doneTimer = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if((enterTrigger && other.CompareTag("Player") && !anythingActivate) || (enterTrigger && anythingActivate) ){
            inside = true;
            if (!timed) {
                ChangeActive();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player") || (anythingActivate)) inside = false;
    }

    public void activateManually() {
        ChangeActive();
    }

    public void activateManuallyTimed() {
        StartCoroutine(waitTimer(timer));
    }

    IEnumerator waitTimer(float duration) {
        yield return new WaitForSeconds(duration);
        ChangeActive();
    }

    private void ChangeActive() {
        objectToEnable.SetActive(enable);
        if (oneAndDone) gameObject.SetActive(false);
        if (alwaysSetOpposite) enable = !enable;
    }

}
