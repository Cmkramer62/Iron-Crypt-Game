using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PositionConstrainter : MonoBehaviour {

    public PositionConstraint constraint;
    public bool triggerOnEnable = false;
    public string specificObject = "";

    private void Start() {
        if (!specificObject.Equals("")) {
            var temp = GameObject.Find(specificObject).GetComponent<PositionConstraint>();
            if (temp != null) constraint = temp;
        }
    }


    private void OnEnable() {
        if (triggerOnEnable) {
            if (!specificObject.Equals("")) {
                var temp = GameObject.Find(specificObject).GetComponent<PositionConstraint>();
                if (temp != null) constraint = temp;
            }
            DeactivateConstraint();
        }
    }


    public void OnTriggerEnter(Collider other) {
        if(!triggerOnEnable) DeactivateConstraint();
    }

    public void DeactivateConstraint() {
        constraint.constraintActive = false;
    }

    public void ActivateConstraint() {
        constraint.constraintActive = true;
    }
}
