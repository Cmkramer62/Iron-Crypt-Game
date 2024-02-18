using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateActivatorItem : MonoBehaviour {

    public GateActivator gateScript;

    private void OnEnable() {
        gateScript.CheckList();
    }

    private void OnDisable() {
        //a way to make it so that if at any point the conditions arent met, the gate
        // 's activation is reset.
        gateScript.CheckList();
    }

}
