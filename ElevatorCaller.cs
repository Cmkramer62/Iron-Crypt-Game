using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ElevatorCaller : MonoBehaviour {
    public Animator safetyAnimator, roomAnimator, doorAnimator;
    public bool elevHere = true;
    private int elevHeight = 0; //-1 for below, 0 for here, and 1 for above.

    private bool doorOpenState, inTransit, doorsMoving, playerInside = false;

    private PositionConstraint constraint;
    public Transform standHere;

    public AudioSource elevRoomTopSource, elevRoomMoveSource, doorSource;
    public AudioClip pingClip, doorClip, elevStartClip, elevEndClip;

    public bool startDownAbove, startUpAbove, startDown, startUp, startInside = false;

    private void Start() {
        constraint = GameObject.FindGameObjectWithTag("Player").GetComponent<PositionConstraint>();
        if (startDown) {
            playerInside = startInside;
            //elevHeight = 1;
            DownElevatorButton();
        } else if(startUp) {
            playerInside = startInside;
            elevHeight = -1;
            UpElevatorButton();
        } else if (startDownAbove) {
            playerInside = startInside;
            elevHeight = 1;
            DownElevatorButton();
        } else if (startUpAbove) {
            playerInside = startInside;
            UpElevatorButton();
        }
    }

    private void Update()
    {
    }

    #region -------------------BUTTON PRESS-----------------------

    public void HailElevatorButton() {
        if (!inTransit && !doorsMoving && elevHere) {
            ShiftDoorState();
        } else if(!inTransit && !doorsMoving) {
            BringElevator();
            if (doorOpenState) {
                CloseDoors();
            }
        }
    }

    public void DownElevatorButton() {
        if(!inTransit && !doorsMoving) StartCoroutine(DownElevatorButtonTimer());
    }
    public void UpElevatorButton() {
        if (!inTransit && !doorsMoving) StartCoroutine(UpElevatorButtonTimer());
    }

    private IEnumerator DownElevatorButtonTimer() {
        if (doorOpenState) {
            CloseDoors();
            yield return new WaitForSeconds(3f);
        }
        if (elevHeight == 1) {
            roomAnimator.Play("ElevMoveDownAbove");
            StartCoroutine(ElevMoveTimer(10f));
            elevHeight = 0;
            elevHere = true;
        } else if (elevHeight == 0) {
            roomAnimator.Play("ElevMoveDown");
            StartCoroutine(ElevMoveTimer(5f));
            elevHeight = -1;
            elevHere = false;
        } else if (!doorOpenState) OpenDoors();
    }

    
    private IEnumerator UpElevatorButtonTimer() {
        if (doorOpenState) {
            CloseDoors();
            yield return new WaitForSeconds(2.5f);
        }
        if (elevHeight == 0) {
            StartCoroutine(ElevMoveTimer(10f));
            yield return new WaitForSeconds(.5f);
            roomAnimator.Play("ElevMoveUpAbove");
            elevHeight = 1;
            elevHere = false;
        } else if (elevHeight == -1) {
            StartCoroutine(ElevMoveTimer(5f));
            yield return new WaitForSeconds(.5f);
            roomAnimator.Play("ElevMoveUp");
            elevHeight = 0;
            elevHere = true;
        } else if (!doorOpenState) OpenDoors();
    }
    #endregion

    #region ----------------DOORS-----------------
    public void ShiftDoorState() {
        if (doorOpenState && !doorsMoving) CloseDoors();
        else OpenDoors();
    }

    private void CloseDoors() {
        if (!doorsMoving) {
            doorOpenState = false;
            StartCoroutine(CloseDoorsTimer());
        }
    }

    private void OpenDoors() {
        if (!doorsMoving) { 
            doorOpenState = true;
            StartCoroutine(OpenDoorsTimer());
        }
    }

    private IEnumerator OpenDoorsTimer() {
        //Barrier.SetActive(false);
        doorSource.PlayOneShot(doorClip);
        doorsMoving = true;
        if (elevHere) safetyAnimator.Play("SafetyBarrierSlide");
        yield return new WaitForSeconds(.5f);
        doorAnimator.Play("InnerDoorsOpen");
        yield return new WaitForSeconds(3.1f);
        doorsMoving = false;
        //inTransition = false;
    }

    private IEnumerator CloseDoorsTimer() {
        doorSource.PlayOneShot(doorClip);
        doorsMoving = true;
        if(elevHere) safetyAnimator.Play("SafetyBarrierSlide 0");
        yield return new WaitForSeconds(.5f);
        doorAnimator.Play("InnerDoorsClose");

        yield return new WaitForSeconds(3.1f);
        doorsMoving = false;
        yield return new WaitForSeconds(1.5f);
    }
    #endregion

    private void BringElevator() {
        if (elevHere) return;
        else {
            if (elevHeight == -1) {
                roomAnimator.Play("ElevMoveUp");
                StartCoroutine(ElevMoveTimer(5f));
                elevHeight = 0;
                elevHere = true;
            }
            else if (elevHeight == 1) {
                roomAnimator.Play("ElevMoveDownAbove");
                StartCoroutine(ElevMoveTimer(10f));
                elevHeight = 0;
                elevHere = true;
            }
            else return;
        }
    }

    private IEnumerator ElevMoveTimer(float duration) {
        inTransit = true;

        if (playerInside) {
            var source = new ConstraintSource();
            source.sourceTransform = standHere;
            source.weight = 1;
            constraint.SetSource(0, source);
            constraint.constraintActive = true;
        }

        StartCoroutine(ElevSoundTimer(duration - 1.35f));
        yield return new WaitForSeconds(duration);
        inTransit = false;
        if (playerInside) constraint.constraintActive = false;
        elevRoomTopSource.PlayOneShot(pingClip);
        OpenDoors();
    }

    private IEnumerator ElevSoundTimer(float duration) {
        elevRoomMoveSource.PlayOneShot(elevStartClip);
        yield return new WaitForSeconds(duration);
        elevRoomMoveSource.Stop();
        elevRoomMoveSource.PlayOneShot(elevEndClip);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) playerInside = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) playerInside = false;
    }

    /*
     * Button press. -> is it here? ->yes, ShiftDoor(). no->callElevator(), then openDoor when here.
     */


    //elevatorHere? returns true if elevator is on floor, false if not.
    //elevatorHail opens doors if elevatorHere, brings elev here if not.
    //elevhail is called by the secondary script-Activator.cs

    //open door
}
