using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchActivator : MonoBehaviour {

    public Animator turnbableAnimator;
    public AudioSource crankSource;
    public AudioClip doneClick;

    public bool activated = false;

    public bool enableObject, cumulativeCranks = false;
    public GameObject[] crankList;

    public float progress = 0;
    public float totalProgressPossible = 50;
    public UltimateCircularHealthBar UIScript;

    private DoorRaycast raycastScript;
    private bool playing = false;
    private float n = 0;
    private bool done = false;

    private void Start() {
        raycastScript = GameObject.Find("Main Camera").GetComponent<DoorRaycast>();
        n = 5 / totalProgressPossible; // n: the amount the UI should remove a frame. 5: the segment count of the UI.
        turnbableAnimator.Play("WrenchableTurn");
        turnbableAnimator.speed = 0;
    }
    
    private void Update() {
        if (!done && raycastScript.wrenching && raycastScript.wrenchableName.Equals(gameObject.name)) { 
            progress += 1 * Time.deltaTime;
            UIScript.RemovedSegments -= n * Time.deltaTime;
            if (!playing) {
                crankSource.Play();
                playing = true;
                turnbableAnimator.speed = 1;
            }
            if (!done && progress >= totalProgressPossible) ActivateEffect();
        } else {
            if (playing) {
                crankSource.Pause();
                playing = false;
                turnbableAnimator.speed = 0;
            }
        }
    }

    public void ActivateEffect() {
        done = true;
        crankSource.Stop();
        crankSource.volume = 1;
        crankSource.PlayOneShot(doneClick);
        Debug.Log("Finished");
        /*
        StartCoroutine(BufferTime(1f));

        if (cumulativeCranks) {
            bool alltrue = true;
            foreach (GameObject crank in crankList) {
                if (!crank.GetComponent<WrenchActivator>().activated) {
                    alltrue = false;
                }
            }
            if (alltrue) {
                if (enableObject) {
                    gameObject.GetComponent<enableObject>().activateManually();
                }
            }
        }

        else if (enableObject) {
            gameObject.GetComponent<enableObject>().activateManually();
        }
        */
    }

    private IEnumerator BufferTime(float duration) { //Buffer time for animation to play.
        yield return new WaitForSeconds(duration);
        activated = true;
    }

}
