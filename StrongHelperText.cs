using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StrongHelperText : MonoBehaviour {
    public static StrongHelperText instance;

    public TextMeshProUGUI UIText;
    public CanvasGroup PopupVisuals;
    private bool inText = false;
    public string temp = "";

    private bool waitingToKill = false;
    private string gm;
    private float dm;

    private bool move = false;
    private int updown = 0;

    // Start is called before the first frame update
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {
        if (instance.waitingToKill && Input.anyKey) {
            Debug.Log("Got");
            instance.StartCoroutine(RemoveMessage(gm,dm));
        }

        if (move && instance.PopupVisuals.alpha != updown) {
            if (instance.PopupVisuals.alpha < updown) instance.PopupVisuals.alpha += 1f * Time.unscaledDeltaTime;
            else instance.PopupVisuals.alpha -= 1f * Time.unscaledDeltaTime;
        }
        else if (move) move = false;
    }

    public static void PopupMessage(string goalMessage, float duration) {
        instance.StartCoroutine(PopupMessageRoutine(goalMessage, duration));
        instance.gm = goalMessage;
        instance.dm = duration;
    }

    private static IEnumerator RemoveMessage(string goalMessage, float duration) {
        Debug.Log("removing");
        instance.waitingToKill = false;

        instance.move = true;
        instance.updown = 0;

        Time.timeScale = 1f;
        PlayerNullify.HardSetAllow();

        foreach (char c in goalMessage + "\nPress any key to continue.") {
            instance.temp = instance.temp.Substring(0, instance.temp.Length - 1);
            instance.UIText.text = instance.temp;
            yield return new WaitForSeconds(.035f);
        }

        
        instance.inText = false;
        instance.UIText.text = "";

        
    }

    private static IEnumerator PopupMessageRoutine(string goalMessage, float duration) {
        instance.move = true;
        instance.updown = 1;

        instance.inText = true;
        instance.temp = "";
        instance.UIText.text = "";

        Time.timeScale = 0f;
        PlayerNullify.HardSetDisallow();
        //GameIsPaused = true;

        foreach (char c in goalMessage) {
            instance.temp += c;
            instance.UIText.text = instance.temp;
            yield return new WaitForSecondsRealtime(.035f);
        }

        yield return new WaitForSecondsRealtime(duration);
        foreach (char c in "\nPress any key to continue.") {
            instance.temp += c;
            instance.UIText.text = instance.temp;
            yield return new WaitForSecondsRealtime(.035f);
        }
        instance.waitingToKill = true;
        //now Any key will start killing hint?
        

        
    }

    
}
