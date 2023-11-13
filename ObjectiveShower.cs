using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveShower : MonoBehaviour {

    public static ObjectiveShower instance;
    public Animator animator;
    public TextMeshProUGUI objectiveText;

    private Coroutine bobby;
    private bool showing = false;
    private bool inText = false;
    private string temp = "";

    public AudioSource source;
    public AudioClip clip;

    void Start() {
        instance = this;
    }

    //Showing custom text.
    public static void ShowText(string text) {
        if (!instance.showing) {
            if(!instance.objectiveText.text.Equals(text)) instance.source.PlayOneShot(instance.clip);

            instance.animator.Play("ShowText");
            instance.showing = true;
            instance.objectiveText.text = text;
            //instance.StartCoroutine(PopupMessageRoutine(text));
        }
    }

    //Show default text.
    public static void ShowText() {
        if (instance.bobby != null) instance.StopCoroutine(instance.bobby);
        if (!instance.showing) {
            //if (!instance.objectiveText.text.Equals(instance.text)) instance.source.PlayOneShot(instance.clip);
            instance.animator.Play("ShowText");
            instance.showing = true;
            //instance.StartCoroutine(PopupMessageRoutine(instance.defaultObjective));
        }
    }

    public static void ResumeText(float duration) {
        if(instance.bobby != null) instance.StopCoroutine(instance.bobby);
        instance.bobby = instance.StartCoroutine(ShowTextTimedCooldown(instance.objectiveText.text, duration));
    }

    public static void HideText() {
        if (instance.showing) {
            instance.animator.Play("HideText");
            instance.showing = false;
            //instance.StartCoroutine(RemoveMessageRoutine(instance.objectiveText.text));
        }
    }

    //Used for showing custom objective.
    public static void ShowTextTimed(string text, float duration) {
        instance.bobby = instance.StartCoroutine(ShowTextTimedCooldown(text, duration));
    }

    //Used for showing default objective.
    public static void ShowTextTimed(float duration) {
        instance.bobby = instance.StartCoroutine(ShowTextTimedCooldown(duration));
    }

    private static IEnumerator ShowTextTimedCooldown(string text, float duration) {
        ShowText(text);
        yield return new WaitForSeconds(duration);
        HideText();
    }

    private static IEnumerator ShowTextTimedCooldown(float duration) {
        ShowText();
        yield return new WaitForSeconds(duration);
        HideText();
    }


    /*
    private static IEnumerator PopupMessageRoutine(string goalMessage) {
        instance.inText = true;
        instance.temp = "";
        instance.objectiveText.text = "";

        foreach (char c in goalMessage) {
            instance.temp += c;
            instance.objectiveText.text = instance.temp;
            yield return new WaitForSecondsRealtime(.035f);
        }
    }

    private static IEnumerator RemoveMessageRoutine(string goalMessage) {
        foreach (char c in goalMessage) {
            instance.temp = instance.temp.Substring(0, instance.temp.Length - 1);
            instance.objectiveText.text = instance.temp;
            yield return new WaitForSeconds(.035f);
        }


        instance.inText = false;
        instance.objectiveText.text = "";
    }
    */
}
