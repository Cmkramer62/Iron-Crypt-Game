using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelperText : MonoBehaviour {

    public static HelperText instance;
    public TextMeshProUGUI UIText;
    public Animator popupAnimator;
    public string temp = "";

    private Coroutine routine;
    private bool inText = false;


    void Awake() {
        instance = this;
    }

    /*
     * PopupMessage() starts a new coroutine named PopupMessageRoutine, and kills any old one.
     */
    public static void PopupMessage(string goalMessage, float duration) {
        if(instance.routine != null) instance.StopCoroutine(instance.routine);
        instance.routine = instance.StartCoroutine(PopupMessageRoutine(goalMessage, duration));
    }

    /*
     * RemoveMessage() sets the display text to nothing, and hides the panel showing the display text.
     */
    public static void RemoveMessage() {
        instance.UIText.text = "";
        instance.popupAnimator.Play("Pop down");
        instance.inText = false;
    }

    /*
     * PopUpMessageRoutine() displays the goalMessage one letter at a time, in increments of .035th of a second.
     * It will hold the message on display for float duration time, then remove the message one letter at a time again.
     */
    private static IEnumerator PopupMessageRoutine(string goalMessage, float duration) {
        if (!instance.inText) instance.popupAnimator.Play("Pop up");
        instance.inText = true;
        instance.temp = "";
        instance.UIText.text = "";
        foreach (char c in goalMessage) {
            instance.temp += c;
            instance.UIText.text = instance.temp;
            yield return new WaitForSeconds(.035f);
        }

        yield return new WaitForSeconds(duration);
        foreach (char c in goalMessage) {
            instance.temp = instance.temp.Substring(0,instance.temp.Length-1);
            instance.UIText.text = instance.temp;
            yield return new WaitForSeconds(.035f);
        }
        RemoveMessage();
    }
}
