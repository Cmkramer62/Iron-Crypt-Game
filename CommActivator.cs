using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommActivator : MonoBehaviour {
    public bool collisionActivated = false;

    public Activator activatorScript;
    private Utility utilityScript;

    private void Start() {
        utilityScript = GameObject.Find("Player Two").GetComponent<Utility>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && collisionActivated)
        ActivateCall();
    }

    public void ActivateCall() {
        utilityScript.StartCommunication(activatorScript);
    }

}
