using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLights : MonoBehaviour
{
    public float lightSpeed = 1.0f;
    [SerializeField] GameObject[] clusterLights;

    public bool actOnStart = false;

    private bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        if (actOnStart)
        {
            StartCoroutine(waitTime(true));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if (!triggered){
            StartCoroutine(waitTime(true));
        }
        
    }

    IEnumerator waitTime(bool active){
        for (int i = 0; i < clusterLights.Length; i++){
            yield return new WaitForSeconds(lightSpeed);
            clusterLights[i].SetActive(active);
        }
    }
}
