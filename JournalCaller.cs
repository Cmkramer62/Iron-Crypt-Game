using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalCaller : MonoBehaviour {

    public int journalNumber;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) GameObject.Find("Helper Systems").GetComponent<JournalAdder>().ActivateJournalEntry(journalNumber);
    }

}
