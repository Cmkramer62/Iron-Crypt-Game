using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorScript : MonoBehaviour {

    #region DOOR VARIABLES
    private Animator doorAnimator;
    public bool doorState, cooldown, hasUnlocked, loadingDoor, stuck = false;
    public int levelNum = 0;
    public string keyCodeName;
    #endregion

    #region AUDIO VARIABLES
    public AudioSource doorSource;
    public AudioClip closeClip, openClip, denyClip, grantedClip;
    #endregion

    #region LOCK MAT VAR
    public GameObject AccessMachineA, AccessMachineB;
    public Material accessMat;
    public Material denyMat;
    #endregion

    public string denyMessage = "I need a key to unlock this...";

    public void Start() {
        if (doorState) {
            doorAnimator.Play("myDoorOpen", 0, 0.0f);
        }
        if (!hasUnlocked) {
            AccessMachineA.GetComponent<MeshRenderer>().material = denyMat;
            AccessMachineB.GetComponent<MeshRenderer>().material = denyMat;
        }
    }

    private void Awake() {
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    /*
     * InteractWithDoor() will attempt to call ShiftDoor() to change the door's state to the opposite state.
     * If the door is locked, or stuck, a message will display to the player.
     */
    public void InteractWithDoor() {
        if (stuck) {
            doorSource.PlayOneShot(denyClip);
            HelperText.PopupMessage(denyMessage, 3);
        } else if (hasUnlocked) {
            ShiftDoor();
        } else if (GameObject.Find("Player Two").GetComponent<Inventory>().InInventory(keyCodeName)) {
            hasUnlocked = true;
            doorSource.PlayOneShot(grantedClip);
            AccessMachineA.GetComponent<MeshRenderer>().material = accessMat;
            AccessMachineB.GetComponent<MeshRenderer>().material = accessMat;
        } else {
            doorSource.PlayOneShot(denyClip);
            HelperText.PopupMessage("I think I need a " + keyCodeName + " to unlock this...", 3);
        }
    }

    /*
     * ForceUnlock() will unlock the door with no restrictions.
     */
    public void ForceUnlock() {
        hasUnlocked = true;
        stuck = false;
        doorSource.PlayOneShot(grantedClip);
        AccessMachineA.GetComponent<MeshRenderer>().material = accessMat;
        AccessMachineB.GetComponent<MeshRenderer>().material = accessMat;
        
    }

    /*
     * ForceLock() will lock the door with full restrictions.
     */
    public void ForceLock() {
        hasUnlocked = false;
        stuck = true;
        doorSource.PlayOneShot(grantedClip);
        AccessMachineA.GetComponent<MeshRenderer>().material = denyMat;
        AccessMachineB.GetComponent<MeshRenderer>().material = denyMat;
    }

    /*
     * ShiftDoor() changes the doors state to the opposite state.
     * (Open->Closed or Closed->Open)
     */
    private void ShiftDoor() {
         if (!cooldown && doorState) {//already open
            doorState = false;
            doorAnimator.Play("myDoorClose");
            doorSource.PlayOneShot(closeClip);
            StartCoroutine(CooldownTimer());
        } else if (!cooldown) {
            if (loadingDoor) {
                LoaderDoor();
                return;
            }
            doorState = true;
            doorAnimator.Play("myDoorOpen");
            doorSource.PlayOneShot(openClip);
            StartCoroutine(CooldownTimer());
        }
    }
    
    /*
     * LoaderDoor() is a method used for doors that have nothing on the other side.
     * The door will play an animation, then quickly transition into the next scene.
     */
    public void LoaderDoor() {
        doorAnimator.Play("HalfOpen");
        doorSource.Play();
        StartCoroutine(CooldownTimer());
        GameObject.Find("Helper Systems").GetComponent<DeathReset>().nextLevel = levelNum;
        GameObject.Find("Helper Systems").GetComponent<DeathReset>().DeathSequenceStart(3);
    }

    /*
     * CooldownTimer() is used to prevent the player from interacting with a door while the door is in animation.
     */
    IEnumerator CooldownTimer() {
        cooldown = true;
        yield return new WaitForSeconds(1);
        cooldown = false;
    }
}