using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour {

    public Transform startPoint;
    public bool start = true;

    // Start is called before the first frame update
    void Start() {
        if (start) {
            GameObject player = GameObject.Find("Player Two");
            player.transform.position = startPoint.position;
            player.transform.rotation = startPoint.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
