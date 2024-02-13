using System.Collections.Generic;
using UnityEngine;

public class idleTargetAI : MonoBehaviour {
    public GameObject idleCapsule;
    public List<Transform> targetsList = new List<Transform>();
    private int i = 0;
    public bool startRoaming, sentryVersion = false;

    // Start is called before the first frame update
    void Start() {
        if(startRoaming) i = Random.Range(0, targetsList.Count);
    }


    void OnTriggerEnter(Collider other){//When enemy touches the next checkpoint
        if (startRoaming && ((!sentryVersion && other.CompareTag("Crawler") || sentryVersion && other.CompareTag("Sentry")) )) {
            if(i != targetsList.Count - 1) {
                i++;
            }
            else i = 0;
            idleCapsule.transform.position = targetsList[i].transform.position;
        }
    }
      
}
