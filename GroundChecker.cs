using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded = false;

    private string currentTag = "Concrete";

    public AudioSource footSource;
    public AudioClip[] normalStepClips, metalStepClips, snowStepClips, ventStepClips, waterStepClips, tileStepClips, carpetStepClips;
    private AudioClip[] playingFromClips;
    /*
     * Goal is to check the ground beneath the user.
     * Change sound of footsteps based on material
     */
    //Types: Concrete, Metal, Wood, Snow, Vent, Water, Tile

    void Start() {
        playingFromClips = normalStepClips;
    }

    private AudioClip GetRandomClip(AudioClip[] footstepList) {
        int Index = Random.Range(0, footstepList.Length);
        return footstepList[Index];
    }

    // Update is called once per frame
    void Update() {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        RaycastHit hit;
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundDistance, groundMask);
        if (isGrounded && !hit.collider.CompareTag(currentTag)){
            currentTag = hit.collider.tag;

            if (currentTag.Equals("Concrete")) playingFromClips = normalStepClips;
            else if (currentTag.Equals("Metal")) playingFromClips = metalStepClips;
            else if (currentTag.Equals("Snow")) playingFromClips = snowStepClips;
            else if (currentTag.Equals("Vent")) playingFromClips = ventStepClips;
            else if (currentTag.Equals("Water")) playingFromClips = waterStepClips;
            else if (currentTag.Equals("Tile")) playingFromClips = tileStepClips;
            else if (currentTag.Equals("Carpet")) playingFromClips = carpetStepClips;
        }
    }


    public void PlaySound(){
        footSource.pitch = (Random.Range(0.8f, 1.15f));
        AudioClip clip = GetRandomClip(playingFromClips);
        footSource.PlayOneShot(clip, .2f);
        footSource.pitch = 1f;
    }
}
