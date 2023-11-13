using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableActivator : MonoBehaviour {

    public GameObject[] items;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnItem(int i, bool state) {
        items[i].SetActive(state);
    }
}
