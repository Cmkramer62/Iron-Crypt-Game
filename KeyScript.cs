using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    public GameObject Object;
    public AudioSource clickSound;
    public AudioClip clip;

    public MeshRenderer meshRenderer;
    public float speed = .5f;
    private float t = 0.0f;
    private int start2 = 0;

    private bool taken = false;

    public void takeKey() {
        clickSound.PlayOneShot(clip, .1f);
        taken = true;
        StartCoroutine(Fade()); 
    }

    public bool isTaken(){
        if (taken) {
            return true;
        } else {
            return false;
        }
    }


    public void Update() {
            Material[] mats = meshRenderer.materials;
            t += Time.deltaTime;
            mats[0].SetFloat("_Cutoff", t * start2);
            meshRenderer.materials = mats;
    }

    private IEnumerator Fade(){
        start2 = 1;
        t = 0.0f;

        yield return new WaitForSeconds(1);
        Object.SetActive(false);
    }

}
