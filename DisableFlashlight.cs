using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFlashlight : MonoBehaviour {
    public bool turnOff = true;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Player Two") {
            if (turnOff && !triggered) {
                triggered = true;
                GameObject.Find("Player Two").GetComponent<Utility>().hasFlashlight = false;
                if (GameObject.Find("Player Two").GetComponent<Utility>().flashlightActive) {
                    GameObject.Find("Player Two").GetComponent<Utility>().flashlightUsage();
                }
            } else if (!turnOff && !triggered) {
                triggered = true;
                GameObject.Find("Player Two").GetComponent<Utility>().hasFlashlight = true;
            }
        }
    }



}
