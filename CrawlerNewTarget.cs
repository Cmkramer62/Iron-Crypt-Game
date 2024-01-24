using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerNewTarget : MonoBehaviour {

    public CrawlerController crawler;
    public bool colliderBased = true;
    public GameObject newTarget;


    private void OnTriggerEnter(Collider other) {
        if (colliderBased && other.CompareTag("Player")) {
            ManualNewTarget();
        }
    }

    public void ManualNewTarget() {
        crawler.AbandonAndNewTarget(newTarget.transform);
    }

}
