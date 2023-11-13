using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchActivator : MonoBehaviour {

    public Animator turnbableAnimator;
    public AudioSource crankSource;
    public AudioClip turnClip;

    public bool activated = false;

    public bool enableObject, cumulativeCranks = false;
    public GameObject[] crankList;


    public void activation() {
        turnbableAnimator.Play("WrenchableTurn");
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
    }

    private IEnumerator BufferTime(float duration) { //Buffer time for animation to play.
        yield return new WaitForSeconds(duration);
        activated = true;
        crankSource.PlayOneShot(turnClip);
    }

}
