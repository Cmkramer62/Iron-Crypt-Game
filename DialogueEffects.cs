using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEffects : MonoBehaviour {

    public DialogueOptions parentOptionsScript;
    private AudioSource twoDimAudioSource;
    public AudioClip hoverClip, clickClip;

    // Start is called before the first frame update
    void Start() {
        twoDimAudioSource = GameObject.Find("--- 2D AUDIO SOURCE ---").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void PlayHoverClip() {
        twoDimAudioSource.PlayOneShot(hoverClip);
    }

    public void PlayClickClip() {
        twoDimAudioSource.PlayOneShot(clickClip);
    }

    public void EndDialogue() {
        parentOptionsScript.GetComponent<DialogueOptions>().StartDeactivateDialogue();
    }


}
