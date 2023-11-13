using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanActivator : MonoBehaviour {

    public bool modifiable = false;
    public bool defaultState = true;

    [SerializeField]
    private Animator fanAnimator;
    [SerializeField]
    private string startFanText;

    private bool state;

    public float speed = 0.2f; //slow

    public AudioSource source;
    public AudioClip activationClip;
    public AudioClip ambientSound;

    void Start() {
        state = defaultState;
        if (defaultState) {
            fanAnimator.Play(startFanText);
            fanAnimator.speed = speed;
            source.PlayOneShot(activationClip);
            //source.PlayOneShot(ambientSound);
            //Play sound at start of activation.
        }
    }

    public void ActivateFan() {

    }

}
