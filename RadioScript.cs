using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{

    public AudioSource mainSource;
    public AudioSource SFXSource;

    public AudioClip playClip;
    public AudioClip stopClip;


    public bool playOnce, pausable, playOnStart, playing;
    private bool done = false;
    // Start is called before the first frame update
    void Start() {
        if(playOnStart) {
            Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region HELPER METHODS
    public void Play() {
        playing = true;
        SFXSource.PlayOneShot(playClip);
        mainSource.Play();
    }

    public void Stop()
    {
        playing = false;
        SFXSource.PlayOneShot(stopClip);
        mainSource.Stop();
    }

    public void Pause() {
        playing = false;
        SFXSource.PlayOneShot(stopClip);
        mainSource.Pause();
    }
    #endregion

    public void Activate() {
        Debug.Log("Activate");
        if (playing && pausable && (!playOnce || !done)) {
            Debug.Log("pause");
            Pause();
        } else if (!playOnce || !done) {
            Debug.Log("play");
            Play();
        } else {
            Stop();
        }
    }
}
