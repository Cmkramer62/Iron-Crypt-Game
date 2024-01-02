using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpewerCycle : MonoBehaviour {

    public bool allowedToSpew = true;
    public bool cyclical = true; // If cyclical is false, the spewer will act simply as a on/off.

    public AudioSource fireSource, lidSource;
    public AudioClip lidClip;
    public VisualEffect fire;
    public GameObject barrier;

    public Animator flapAnimator;

    public float effectDuration, downtimeDuration = 1f;

    public bool firing = false;

    private Coroutine cycleRoutine;

    // Start is called before the first frame update
    void Start() {
        if (allowedToSpew && cyclical) StartCycle();
        else if (allowedToSpew) ActivateManually();
    }

    public void SwapManually() {

        if (cyclical) {
            if (!allowedToSpew) StartCycle();
            else {
                StopCoroutine(cycleRoutine);
                DeactivateFire();
            }
            allowedToSpew = !allowedToSpew;
        } else {
            if (firing) DeactivateFire();
            else ActivateFire();
        }

    }

    public void ShutDownManually() {
        if(firing) DeactivateFire();
        if (cyclical) allowedToSpew = false;
    }

    public void ActivateManually() {
        if(!firing) ActivateFire();
        if (cyclical) {
            StartCycle();
            allowedToSpew = true;
        }
    }

    public void StartCycle() {
        if(cyclical) cycleRoutine = StartCoroutine(CycleTimer());
    }

    private IEnumerator CycleTimer() {
        ActivateFire();
        yield return new WaitForSeconds(effectDuration);
        DeactivateFire();
        yield return new WaitForSeconds(downtimeDuration);

        if(allowedToSpew) StartCycle();
    }

    private void ActivateFire() {
        lidSource.PlayOneShot(lidClip);
        flapAnimator.Play("LidOpen");
        fireSource.Play();
        barrier.SetActive(true);
        fire.Play();
        firing = true;
    }

    private void DeactivateFire() {
        fireSource.Stop();
        barrier.SetActive(false);
        fire.Stop();//or pause..? whats the difference?

        lidSource.PlayOneShot(lidClip);
        flapAnimator.Play("LidClose");
        firing = false;
    }

    //play steam clip
    //wait X seconds
    //stop steam clip
    //play fire clip
    //turn on fire, hurt zone, and barrier (steam from other sections wouldnt make sense otherwise). Also make barrier thin to avoid clipping.
    //wait y seconds
    //turn off fire
    //turn off fire clip
    //repeat



}
