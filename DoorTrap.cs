using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    private bool triggered = false;
    public Animator bobby;
    public AudioClip clip;
    public AudioSource mySound;

    void Update() {
        if(GameObject.Find("trappedDoor").GetComponent<DoorScript>().doorState == true && triggered == false) {
            Debug.Log("Jumpscare!");
            bobby.Play("JS1_1", 0, 0.0f);
            triggered = true;
            mySound.PlayOneShot(clip);
        }
    }

}
