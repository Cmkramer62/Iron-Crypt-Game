using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Jumpscare2Trigger : MonoBehaviour {
    public GameObject myself;//the ghoul this is attached to.
    public Transform lookAt;
    public int deathType = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Player Two") {
            myself.SetActive(false);
            GameObject.Find("Helper Systems").GetComponent<Jumpscare2>().Jumpscare(lookAt, deathType);
        }
    }
}
