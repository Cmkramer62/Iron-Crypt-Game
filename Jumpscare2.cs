using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class Jumpscare2 : MonoBehaviour {

    public GameObject JSGhoul;
    public Transform baseLine;//can be anything that is facing horizontally.
    public Animator fade;
    public GameObject hint;

    public LookAtConstraint cameraLAC;
    private ConstraintSource look;

    #region HINT VARS
    public TextMeshProUGUI denyScript;
    public string denyMessage = "I need a key to unlock this...";
    public string temp = "";
    #endregion

    #region SOUND VARS
    public AudioSource source;
    public AudioClip clip;
    #endregion

    //add arguments here to specify what kind of death it is,-> so we know what kind of hint to show.
    public void Jumpscare(Transform lookAt) { //separated because there are other ways to kill, not just touch.
        StartCoroutine(JumpscareTime(lookAt));
    }

    IEnumerator JumpscareTime(Transform lookAt) {
        GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = false;
        GameObject.Find("Player Two").GetComponent<PlayerMovement2>().allowedToMove = false;
        //GameObject.Find("Main Camera").transform.rotation = baseLine.rotation;
        
        look.sourceTransform = lookAt;
        look.weight = 1;
        cameraLAC.SetSource(0, look);
        cameraLAC.constraintActive = true;

        GameObject.Find("Helper Systems").GetComponent<UIController>().canPause = false;
        JSGhoul.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        source.PlayOneShot(clip);
        fade.Play("FadeIn3");
        yield return new WaitForSeconds(2.3f);
        hint.SetActive(true);
        StartCoroutine(waitTime3()); //display hint
        yield return new WaitForSeconds(8);
        GameObject.Find("Helper Systems").GetComponent<DeathReset>().DeathSequenceStart(0);
    }

    IEnumerator waitTime3() {
        if (temp.Equals(denyMessage)){
            temp = "";
        }
        foreach (char c in denyMessage){
            temp = temp + c;
            denyScript.text = temp;
            yield return new WaitForSeconds(.035f);
        }
    }

}
