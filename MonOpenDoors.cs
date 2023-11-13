using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonOpenDoors : MonoBehaviour {

    private DoorScript doorScript;
    // Start is called before the first frame update
    void Start() {
        doorScript = gameObject.GetComponentInChildren<DoorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerStay(Collider other) {
        if (other.CompareTag("Crawler") && doorScript.hasUnlocked == true && !doorScript.doorState && other.GetComponent<CrawlerController>().hunting) {
            doorScript.InteractWithDoor();
        }
    }
}
