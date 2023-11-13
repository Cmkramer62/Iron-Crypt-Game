using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTarget : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public Transform transportee;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void teleport() {
        transportee.position = target.position;
    }
}
