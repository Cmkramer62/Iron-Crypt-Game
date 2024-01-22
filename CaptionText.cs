using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionText : MonoBehaviour {

    public bool doOnce = true;
    private bool done = false;
    public float duration = 5f;
    public float interDuration = 2f;

    public string[] captionText;

    private void OnEnable() {
        if(doOnce && !done || !doOnce) StartCoroutine(ActivateHint());
    }

    private IEnumerator ActivateHint() {
        foreach (string caption in captionText) {
            HelperText.PopupMessage(caption, duration);
            yield return new WaitForSeconds(interDuration);
        }
    }

}
