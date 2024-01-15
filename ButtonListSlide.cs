using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class ButtonListSlide : MonoBehaviour {
    public float introLen = 1f;
    [SerializeField] GameObject[] buttons;

    // Start is called before the first frame update
    public void Start() {
        StartCoroutine(ButtonAnimationCooldowns());
    }

    private IEnumerator ButtonAnimationCooldowns() {
        yield return new WaitForSecondsRealtime(.5f);
        Debug.Log("Starting");
        for (int i = 0; i < buttons.Length; i++) {
            yield return new WaitForSecondsRealtime(introLen);
            buttons[i].SetActive(true);
        }
    }

    public void resetButtons() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].SetActive(false);
        }
    }

    public void blurCamera()
    {
        GameObject.Find("Main Camera").GetComponent<VignetteAndChromaticAberration>().blur = 1.0f;
    }


}
