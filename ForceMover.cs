using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ForceMover : MonoBehaviour {

    private GameObject player;
    private Animator animator;
    private LookAtConstraint cameraLAC;

    public string[] animationNames;
    public bool[] autoCameraActive; //List of bools that corresponds with the animations. 1-1 ratio.
    public float[] animationLengths;

    public bool playAudio;
    public AudioSource[] audioSources;

    public Transform transformPoint, lookAtPoint;
    public bool lookAtActivated = true;

    void Start() {
        player = GameObject.Find("Player Two");
        animator = gameObject.GetComponentInChildren<Animator>();
        cameraLAC = GameObject.Find("Main Camera").GetComponent<LookAtConstraint>();
    }

    private void OnTriggerEnter(Collider other) {
        //NOTE: if this is OTE enabled, disable the Mover's "Interact" tag.
        if(other.CompareTag("Player") && !lookAtActivated) MoverProcedure();
    }

    public void MoverProcedure() {
        PlayerNullify.ChangePlayerAbilityPerms();
        player.transform.position = transformPoint.position;

        player.transform.parent = transformPoint;
        StartCoroutine(AnimationTimer());
    }

    private IEnumerator AnimationTimer() {
        int i = 0;

        foreach (string animation in animationNames) {
            animator.Play(animationNames[i]);
            if(playAudio && i < audioSources.Length && audioSources[i] != null) audioSources[i].Play();
            if (autoCameraActive[i]) {
                cameraLAC.constraintActive = true;
                ConstraintSource source = new ConstraintSource();
                source.sourceTransform = lookAtPoint;
                source.weight = 1;
                cameraLAC.SetSource(0, source);
            } else { 
                PlayerNullify.PlayerAbilitiesSetCameraException(true);
            }
            yield return new WaitForSeconds(animationLengths[i]);
            i++;
            cameraLAC.constraintActive = false;
        }
        PlayerNullify.HardSetAllow();
        player.transform.parent = GameObject.Find("---BUNDLE---").transform;
    }

}
