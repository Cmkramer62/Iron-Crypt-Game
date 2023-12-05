using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReShrinkGoHome : MonoBehaviour {

    public GameObject[] cards;
    public GameObject[] dataImages;
    public TextMeshProUGUI tmpText;

    public AudioSource audioSource;
    public AudioClip hoverFilledClip;
    public AudioClip hoverEmptyClip;

    public void ShrinkRest(int index) {
        if (cards[index].GetComponent<DexDisplay>().dataAchieved) {
            tmpText.text = cards[index].GetComponent<DexDisplay>().informationText;
            cards[index].GetComponent<DexDisplay>().dataVisuals.SetActive(true);
            audioSource.PlayOneShot(hoverFilledClip);
        } else {
            tmpText.text = "???";
            audioSource.PlayOneShot(hoverEmptyClip);
        }

        int i = 0;
        foreach(GameObject card in cards) {
            if (i != index) {
                card.GetComponent<Animator>().ResetTrigger("goBig");
                card.GetComponent<Animator>().SetTrigger("goHome");
                card.GetComponent<DexDisplay>().dataVisuals.SetActive(false);
            }
            i++;
        }
    }
}
