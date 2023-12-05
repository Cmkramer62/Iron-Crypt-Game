using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCabinetUse : MonoBehaviour {

    public bool open = false;
    public AudioSource cabinetSource;
    public AudioClip openClip, closeClip;

    public GameObject StarParticle;
    public bool playerInside = false;

    //bool locked = false;
    private bool ableToOpen = true;
    private Animator cabinetAnimator;
    private bool firstTimeOpened = true;
    public GameObject waitTransform;

    // Start is called before the first frame update
    void Start() {
        cabinetAnimator = gameObject.GetComponent<Animator>();
    }

    public void activateCabinet() {
        if(firstTimeOpened) { // De-activate the guidance Star when player opens.
            StarParticle.SetActive(false);
            firstTimeOpened = false;
        }

        if (!open && ableToOpen) {
            cabinetAnimator.Play("CabinetDoorsOpen");
            open = true;
            cabinetSource.PlayOneShot(openClip);
            StartCoroutine(animationTime());
        } else if (ableToOpen) {
            cabinetAnimator.Play("CabinetDoorsClose");
            open = false;
            cabinetSource.PlayOneShot(closeClip);
            StartCoroutine(animationTime());

            StartCoroutine(AnimationTimeAmbush());

        }
    }

    //Amount of time for the animation to finish. Cannot open/close door in that period.
    private IEnumerator animationTime() {
        ableToOpen = false;
        yield return new WaitForSeconds(1f);
        ableToOpen = true;
    }

    private IEnumerator AnimationTimeAmbush() {
        yield return new WaitForSeconds(1f);
        GameObject crawler = GameObject.FindWithTag("Crawler");
        if (playerInside && crawler.GetComponent<LineOfSightChecker>().CanSeeTarget(gameObject.transform)) {
            crawler.GetComponent<CrawlerController>().waitTarget = waitTransform.transform;
            crawler.GetComponent<CrawlerController>().RandomAmbush(crawler.GetComponent<CrawlerController>().ambushChance);
        }
    }

}
