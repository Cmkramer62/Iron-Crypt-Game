using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateActivator : MonoBehaviour {

    public Activator activatorScript;
    public bool allTrueOrFalse = true;

    public GameObject[] ListOfObjects;

    public void CheckList() {
        bool allCheckedOut = true;
        foreach(GameObject effectee in ListOfObjects) {
            if(  (allTrueOrFalse && !effectee.activeSelf)  ||  (!allTrueOrFalse && effectee.activeSelf)  ) allCheckedOut = false;
        }

        if (allCheckedOut) {
            FinalActivation();
            //Debug.Log("All good");
        }
        //else Debug.Log("not good");
    }

    public void FinalActivation() {
        activatorScript.ActivateSwitch();
    }
}
