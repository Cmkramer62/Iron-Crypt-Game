using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPower : MonoBehaviour {

    public GameObject[] LightsList;
    public GameObject ResetPowerUI, PostResetUI;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }


    public void ForceResetPower() {
        StartCoroutine(ResetTimer());
    }

    private IEnumerator ResetTimer() {
        yield return new WaitForSeconds(4f);
        foreach(GameObject light in LightsList) {
            light.GetComponent<LightFlicker>().TurnOffLight(true);
        }
        ResetPowerUI.SetActive(false);

        yield return new WaitForSeconds(3f);

        foreach (GameObject light in LightsList) {
            light.GetComponent<LightFlicker>().TurnOnLight();
        }
        PostResetUI.SetActive(true);
    }
}
