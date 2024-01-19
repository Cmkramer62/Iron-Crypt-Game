using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PositionConstrainter : MonoBehaviour {

    public PositionConstraint constraint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        DeactivateConstraint();
    }

    public void DeactivateConstraint() {
        constraint.constraintActive = false;
    }

    public void ActivateConstraint() {
        constraint.constraintActive = true;
    }
}
