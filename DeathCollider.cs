using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour {
    public int type = 1;
    public int sceneNumber = 3;

    public void OnTriggerEnter(Collider other) {
        if (other.name == "Player Two") {
            GameObject.Find("Helper Systems").GetComponent<DeathReset>().nextLevel = sceneNumber;
            GameObject.Find("Helper Systems").GetComponent<DeathReset>().DeathSequenceStart(type);
        }
    }
}
