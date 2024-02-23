using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrap : MonoBehaviour
{

    public AudioClip clip;
    public AudioSource mySound;

    public bool doOnce = true;
    private bool ableToPlay = true;
    private bool done = false;
    public bool anythingActivate = false;

    public float cooldown = 0f; // Calls cooldown if > 0f;

    public bool timed, twoDim = false;
    public float timer = 30f;
    private bool inside = false;

    private void Update() {
        if (timed && inside) {
            timer -= 1 * Time.deltaTime;
            if (timer <= 0) {
                if (!doOnce || !done) {
                    PlaySound();
                    if (doOnce) done = true;
                    timed = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){//When player touches object
        if (other.CompareTag("Player") || (anythingActivate && other.CompareTag("Enemy"))) {
            if (!timed && (!doOnce || !done) && ableToPlay) {
                PlaySound();
            }
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        inside = false;
    }

    private void PlaySound() {
        if (doOnce) done = true;
        if (!twoDim) mySound.PlayOneShot(clip);
        else GameObject.Find("--- 2D QUIET SOURCE ---").GetComponent<AudioSource>().PlayOneShot(clip);
        if (cooldown > 0f) StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown() {
        ableToPlay = false;
        yield return new WaitForSeconds(cooldown);
        ableToPlay = true;
    }

}