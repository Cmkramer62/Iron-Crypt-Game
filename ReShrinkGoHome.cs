using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReShrinkGoHome : MonoBehaviour {

    public GameObject[] cards;
    public string[] contentTextList;
    public TextMeshProUGUI tmpText;

    public void ShrinkRest(int index) {
        tmpText.text = contentTextList[index];
        int i = 0;
        foreach(GameObject card in cards) {
            if (i != index) {
                card.GetComponent<Animator>().ResetTrigger("goBig");
                card.GetComponent<Animator>().SetTrigger("goHome");
            }
            i++;
        }
    }
}
