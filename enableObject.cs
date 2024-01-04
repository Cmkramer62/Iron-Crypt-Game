using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableObject : MonoBehaviour {
    public GameObject objectToEnable;
    public bool enable = false;

    public bool enterTrigger = true;
    public bool timed = false;
    public float timer = 30f;
    public bool repeat = false;

    private bool inside = false;
    private bool doneTimer = false;

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
        if(enterTrigger && other.CompareTag("Player")) {
            inside = true;
            if (!timed) {
                objectToEnable.SetActive(enable);
                if (repeat) enable = !enable;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        inside = false;
    }

    public void activateManually() {
        objectToEnable.SetActive(enable);
        if (repeat) enable = !enable;
    }

    public void activateManuallyTimed() {
        StartCoroutine(waitTimer(timer));
    }

    IEnumerator waitTimer(float duration) {
        yield return new WaitForSeconds(duration);
        objectToEnable.SetActive(enable);
        if (repeat) enable = !enable;
    }

}
