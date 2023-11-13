using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator cameraAnimator;
    public string[] animNameList;

    // Start is called before the first frame update
    void Start() {
        cameraAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(int animNum) {
        cameraAnimator.Play(animNameList[animNum]);
    }
}
