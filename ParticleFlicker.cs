using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleFlicker : MonoBehaviour
{
    
    public float minActiveSeconds = 1;
    public float maxActiveSeconds = 2;
    public int minDeactiveSeconds = 1;
    public int maxDeactiveSeconds = 5;
    public ParticleSystem source; 
    public float initWait = 5f; //determines likelihood of flickering

    public AudioSource particleSound;

    //private bool noFlicker = false;
    //private bool temp = false;
    

    public void Start() {
       StartCoroutine(Flicker());
    }
    
    private IEnumerator Flicker() {
        source.Stop();
        yield return new WaitForSeconds(initWait);

        while (true) {
            source.Play();
            particleSound.Play();
            yield return new WaitForSeconds(Random.Range(minActiveSeconds, maxActiveSeconds));
            source.Stop();
            particleSound.Pause();
            yield return new WaitForSeconds(Random.Range(minDeactiveSeconds,maxDeactiveSeconds));


        }
    }
    
}



