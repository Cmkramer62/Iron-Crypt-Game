using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommActivator : MonoBehaviour {
    public bool collisionActivated = false;

    private Utility utilityScript;

    public GameObject[] audioList; //x 
    public float[] timingList; //y
    //eg. y, x, y, x
    public bool needsReference = false;
    public CommActivator referenceCommunicator;
    [Header("Below should be empty.")]
    public GameObject notif, talk, def;
    public bool assignedAtStart = false;

    private bool done = false;

    private void Start() {
        Debug.Log("start " + gameObject.name);
        
        utilityScript = GameObject.Find("Player Two").GetComponent<Utility>();
        notif = GameObject.Find("CommScreenNotif");
        talk = GameObject.Find("CommScreenTalk");
        def = GameObject.Find("CommScreenOff");
        
        if (assignedAtStart) {
            talk.SetActive(false);
            notif.SetActive(false);
            GameObject.Find("Communicator Parent").SetActive(false);
        }
        if (needsReference) {
            utilityScript = GameObject.Find("Player Two").GetComponent<Utility>();
            notif = referenceCommunicator.notif;
            talk = referenceCommunicator.talk;
            def = referenceCommunicator.def;
        }
       
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && collisionActivated)  ActivateCall();
    }

    public void ActivateCall() {
        if (!done) { 
            done = true;
            utilityScript.StartCommunication(gameObject.GetComponent<CommActivator>());
            //communicator screen turn on
            notif.SetActive(true);
            def.SetActive(false);
            talk.SetActive(false);
        }
    }

    public void PlaySequencially() {
        StartCoroutine(PSTimer());
    }

    private IEnumerator PSTimer() {
        //swap to the communicator if not already there.
        //able to swamp = false
        //turn off communicator screen.
        talk.SetActive(true);
        notif.SetActive(false);
        for (int i = 0; i < timingList.Length; i++) {
            yield return new WaitForSeconds(timingList[i]);
            audioList[i].SetActive(true);
        }
        notif.SetActive(false);
        talk.SetActive(false);
        def.SetActive(true);


        //able to swap = true
    }

}
