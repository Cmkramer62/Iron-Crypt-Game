using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrap : MonoBehaviour {

    // Animator- track, lookat point, and subject?
    // track may be -> A. Stationary, B. Nonexistent, C. moving.
    // lookat point -> A. Stationary, B. Nonexistent, C. moving, D. the subject
    // Subject      -> A. Stationary, B. Nonexistent, C. moving.

    //monster jumpscare (Processing & Ventilation Shaft 3): 1-B, 2-B, 3-C. Could be 1-B, 2-A/D, 3-C
    //climbing ladder: 1-C, 2-C, 3-B
    public Animator animator;
    public string animName;
    public bool doOnce = false;
    private bool done = false;
    public AudioSource twoDimSource;
    public AudioSource[] otherSources;
    public AudioClip[] clip;
    public bool cameraShake = false;
    /*
     *  player enters hitbox
     *  subject animation play
     *  noise play
     *  
     * 
     *  disbable camera movement input. disable movement input? 
     *  set slow lookat transform target (same one the CCTV uses) on mainCamera, to subject
     *  enable slowLookat.
     *  StartCoroutine, wait for X seconds.
     */

    /* Simple version
     * touch hitbox if 'able == true' (allows for modularity, if we want JS to be random, or only happen after certain events, etc.)
     * play sound
     * play animation
     * 
     * 
     */


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !done) {
            if (doOnce) done = true;
            animator.Play(animName);
            foreach(AudioClip thisCLip in clip) {
                twoDimSource.PlayOneShot(thisCLip);
            }
            foreach(AudioSource thisSource in otherSources) {
                thisSource.Play();
            }

            if(cameraShake) GameObject.Find("Camera Parent").GetComponent<CameraShake>().PlayAnimation(0);
        }
    }
}
