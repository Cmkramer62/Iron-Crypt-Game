using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFader : MonoBehaviour {

    public AudioSource audioSource;
    public bool collisionActivated = false;
    public bool fadeIn = true; // false means it will unfade the audio.


    private bool working = false; // while working is true, dont accept any commands.

    private bool goUp, goDown = false;

    // Update is called once per frame
    void Update() {
        if (goUp) {
            audioSource.volume += (.1f * Time.deltaTime);
            if(audioSource.volume == 1) {
                goUp = false;
                working = false;
            }
        } else if (goDown) {
            audioSource.volume -= (.1f * Time.deltaTime);
            if (audioSource.volume == 0) {
                goDown = false;
                working = false;
            }
        }
    }

    public void ActivateFade() {
        if (!working) {
            if (fadeIn) goUp = true;
            else goDown = true;
            working = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(collisionActivated && other.CompareTag("Player")) {
            ActivateFade();
        }
    }


}
