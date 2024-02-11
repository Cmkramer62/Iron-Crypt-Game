using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTargetSwap : MonoBehaviour {
    public SentryController sentryScript;
    public GameObject target;

    private bool done = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !done) {
            sentryScript.GoToTarget(target);
            done = true;
        }
    }
}
