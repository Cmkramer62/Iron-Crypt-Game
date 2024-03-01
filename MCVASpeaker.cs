using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCVASpeaker : MonoBehaviour {
    /*
     * Esssentially the gist is that the player only has one mouth,
     * so now that we are adding VA to the MC, we need to assure that
     * he doesn't say two things at once. The solution is to funnel
     * every voice line through this script. Some lines will take priority over others.
     * when this happens, the previous line must be cut-off and stopped.
     * if a lower prio line tries to Play() when a higher prio line is playing,
     * it will simply not play at all. EX: player is saying a dialogue, and
     * touches a flame. No "ouch" sound is played.
     * 
     * Damage "ouch" must be moved here.
     * 
     * This can be made static?
     */

    public bool isTalking = false;
    public AudioSource soundSource;
    [SerializeField] AudioClip[] painSounds;
    [SerializeField] AudioClip[] commQuery;

    // Returns a random audio clip from the list named "painSounds".
    private AudioClip GetRandomClip(AudioClip[] clipList) {
        return clipList[Random.Range(0, clipList.Length)];
    }

    public void RandomPainSound() {
        if (!isTalking) {
            isTalking = true;
            AudioClip clip = GetRandomClip(painSounds);
            soundSource.PlayOneShot(clip);
            StartCoroutine(ClipCooldown(clip.length));
        }
    }

    public void RandomCommQuerySound() {
        if (!isTalking) {
            isTalking = true;
            AudioClip clip = GetRandomClip(commQuery);
            soundSource.PlayOneShot(clip);
            StartCoroutine(ClipCooldown(clip.length));
        }
    }

    private IEnumerator ClipCooldown(float duration) {
        yield return new WaitForSeconds(duration);
        isTalking = false;
    }



}
