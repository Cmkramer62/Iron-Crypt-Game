using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReShrinkGoHome : MonoBehaviour {

    public GameObject[] cards;

    public void ShrinkRest(int index) {
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
