using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monRanSound : MonoBehaviour {

    [SerializeField] AudioClip[] audioClip;
    public bool allowed = true;
    public AudioSource myAudioSource;

    private void Start() {
        if (allowed) StartCoroutine(RandomSound());
    }

    private IEnumerator RandomSound() {
        if (allowed) myAudioSource.PlayOneShot(audioClip[Random.Range(0, audioClip.Length)], .3f);
        yield return new WaitForSeconds(Random.Range(2.5f, 15));
        StartCoroutine(RandomSound());
    }
}
