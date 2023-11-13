using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    // If true, 'E' must be pressed to activate. Otherwise, activation is called upon OnTriggerEnter.
    public bool manualActivation = true;
    public bool ableToUse = true;
    public bool doOnce = false;
    public float cooldown = 0f;
    public float timed = 0f; // Time delay between each subsequent activation.

    public Animator switchAnimator;
    public string animNameOn, animNameOff; // Name both same thing if only one effectee.
    public GameObject[] effectee;

    public AudioSource sourceSound;
    public AudioClip onClip, offClip, latchClip; // Set both same clip if only one sound needed.

    public GameObject star;
    private bool cooldownDone = true;

    public bool useSFX = true;
    public bool useVFX = true;
    //public bool state = false;

    private IEnumerator Activate() {
        if (ableToUse && cooldownDone) {
            if (doOnce) ableToUse = false;
            else if (cooldown > 0f) StartCoroutine(CooldownTime());

            if (useVFX && star.activeSelf) star.SetActive(false);

            if (useSFX && effectee[0].activeSelf) {
                if(useVFX) switchAnimator.Play(animNameOn, 0, 0.0f);
                sourceSound.PlayOneShot(onClip);
                //StartCoroutine(LatchSound());
            } else if(useSFX) {
                if (useVFX) switchAnimator.Play(animNameOff, 0, 0.0f);
                sourceSound.PlayOneShot(offClip);
                if (effectee.Length > 1) StartCoroutine(LatchSound());
            }
            foreach (GameObject listItem in effectee) {
                if (listItem.CompareTag("Particle")) {
                    if (listItem.GetComponent<ParticleSystem>().isPlaying)
                        listItem.GetComponent<ParticleSystem>().Stop();
                    else listItem.GetComponent<ParticleSystem>().Play();
                }
                else if (listItem.CompareTag("AutoDoor")) listItem.GetComponent<AutoDoor>().ShiftDoor();
                else if (listItem.CompareTag("Elevator")) {
                    if (gameObject.name.Equals("Button Parent (Down)")) listItem.GetComponent<ElevatorCaller>().DownElevatorButton();
                    else if (gameObject.name.Equals("Button Parent (Up)")) listItem.GetComponent<ElevatorCaller>().UpElevatorButton();
                    else listItem.GetComponent<ElevatorCaller>().HailElevatorButton();
                }
                else listItem.SetActive(!listItem.activeSelf);
                yield return new WaitForSeconds(timed);
            }
        }
    }

    #region ----------------------- HELPER METHODS

    public void ForceActivateSwitch() {
        StartCoroutine(Activate());
    }

    public void ActivateSwitch() {
        if (manualActivation) {
            StartCoroutine(Activate());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!manualActivation) {
            StartCoroutine(Activate());
        }
    }

    private IEnumerator CooldownTime() {
        cooldownDone = false;
        yield return new WaitForSeconds(cooldown);
        cooldownDone = true;
    }
    
    private IEnumerator LatchSound() {
        yield return new WaitForSeconds(cooldown);
        sourceSound.PlayOneShot(latchClip);
    }
    #endregion

}
