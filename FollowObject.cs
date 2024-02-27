using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FollowObject : MonoBehaviour {

    private GameObject targetToFollow;
    public string targetName = "temp name";

    public string followerName = "temp name";

    // Start is called before the first frame update
    void OnEnable() {
        targetToFollow = GameObject.Find(targetName);
        ConstraintSource placeholderSource = new ConstraintSource();
        placeholderSource.sourceTransform = targetToFollow.transform;
        placeholderSource.weight = 1;

        if (gameObject.GetComponent<PositionConstraint>()) {
            gameObject.GetComponent<PositionConstraint>().SetSource(0, placeholderSource);
            gameObject.GetComponent<PositionConstraint>().constraintActive = true;
        } else {
            GameObject temp = GameObject.Find(followerName);
            temp.GetComponent<PositionConstraint>().SetSource(0, placeholderSource);
            temp.GetComponent<PositionConstraint>().constraintActive = true;
        }
        
    }

    private void OnDisable() {
        if (gameObject.GetComponent<PositionConstraint>()) {
            gameObject.GetComponent<PositionConstraint>().constraintActive = false;
        } else {
            GameObject temp = GameObject.Find(followerName);
            temp.GetComponent<PositionConstraint>().constraintActive = false;

        }
    }
}
