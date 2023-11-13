using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEffectTrigger : MonoBehaviour {

    public SentryController sentryScript;
    // 0-GlobalAlarmTrip, 1-GlobalAlarmDisable, 2-LocalAlarmTrip, 3-projection
    public int effectTypeNumber = 0;
    // 0-SameMode, 1-IdleStill, 2-IdleFollowPlayer, 
    public int newModeNumber = 0;

    private void OnTriggerEnter(Collider other) {
        if(other.name.Equals("Player Two") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))   ) {
            sentryScript.ActivateEffect(effectTypeNumber);
            if (GameObject.Find("SentryLoopChanger")) {
                GameObject.Find("SentryLoopChanger").GetComponent<SentryLoopChanger>().ableToLoop = false;
            }

            if (newModeNumber == 1) {
                sentryScript.IdleMode(0);
            } else if(newModeNumber == 2) {
                sentryScript.IdleMode(2);
            }
        }
    }
}
