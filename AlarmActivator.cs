using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmActivator : MonoBehaviour {

    public GameObject[] alarms;
    private bool doneOnce = false;

    private void OnTriggerEnter(Collider other) {
        if(!doneOnce && other.CompareTag("Player")) {
            doneOnce = true;
            foreach (GameObject alarm in alarms) {
                alarm.GetComponent<AlarmLights>().AlarmChange(true);
            }
        }
    }

    private void ManualAlarmChange(bool activeBool) {
        if(!doneOnce) {
            doneOnce = true;
            foreach (GameObject alarm in alarms) {
                alarm.GetComponent<AlarmLights>().AlarmChange(activeBool);
            }
        }
    }

    public void ManualAlarmOff() {
        ManualAlarmChange(false);
    }

    public void ManualAlarmOn() {
        ManualAlarmChange(true);
    }
}
