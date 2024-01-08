using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNullifyAuto : MonoBehaviour {

    public bool simpleNullify = false;

    private void Start() {
         PlayerNullify.PlayerAbilitiesSetMovementException(false);
    }

    private void OnDisable() {
        Debug.Log("Disable");
        if (!simpleNullify) PlayerNullify.HardSetAllow();
        else PlayerNullify.SimpleHardSetAllow();
    }
}
