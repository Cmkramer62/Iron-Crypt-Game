using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalAdder : MonoBehaviour {

    public GameObject[] journalList;

    public void ActivateJournalEntry(int index) {
        journalList[index].SetActive(true);
    }

}
