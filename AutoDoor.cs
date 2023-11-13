using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour {

    public bool ableToOpen, automatic = true;
    public Animator doorAnimator;
    public bool open = false;
    public AudioSource doorSource;
    public AudioClip openClip;
    public AudioClip closeClip;
    
    public void ShiftDoor() {
        if (ableToOpen && !open) {
            OpenDoor();
        }
        else if (ableToOpen) CloseDoor();
    }

    private void OpenDoor() {
        doorAnimator.Play("SciDoorOpen");
        doorSource.PlayOneShot(openClip);
        open = true;
    }

    private void CloseDoor() {
        doorAnimator.Play("SciDoorClose");
        doorSource.PlayOneShot(closeClip);
        open = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (automatic && ableToOpen && !open) {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (automatic && ableToOpen && open) {
            CloseDoor();
        }
    }

}
