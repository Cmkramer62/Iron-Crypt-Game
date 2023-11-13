using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLights : MonoBehaviour {

    public bool active = false;
    public GameObject lights;
    public Animator alarmAnimator;
    public AudioSource alarmSource;
    public Renderer bulbOne, bulbTwo;
    public Material onMat, offMat;

    // Start is called before the first frame update
    void Start() {
        
        AlarmChange(active);
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void AlarmChange(bool activated) {
        active = activated;
        
        lights.SetActive(active);
        if (active) {
            alarmAnimator.Play("Vertical Spinning");
            alarmSource.Play();
            bulbOne.material = onMat; bulbTwo.material = onMat;
        } else {
            alarmAnimator.Play("Stopped");
            alarmSource.Stop();
            bulbOne.material = offMat; bulbTwo.material = offMat;
        }
    }
}
