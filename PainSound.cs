using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainSound : MonoBehaviour{

    public  AudioSource soundSource;
    [SerializeField]  AudioClip[] painSounds;

    // Returns a random audio clip from the list named "painSounds".
    private  AudioClip GetRandomClip(){
        return painSounds[Random.Range(0, painSounds.Length)];
    }

    public  void playRandomPainSound() {
        soundSource.PlayOneShot(GetRandomClip());
    }


   
}
