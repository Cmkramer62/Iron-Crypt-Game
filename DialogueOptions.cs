using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptions : MonoBehaviour {

    public string[] listOfDialogue;
    public GameObject[] boxesList;
    public GameObject boxPrefab;

    

    // list of strings. "yes", "no", "Im not sure";
    // on Activation()-> nullify the player. bring out mouse. disable ability to pause. 
    // for each string in list, total int. GameObject.Add(prefab for boxes) as a child of the grid. fade in one at a time, .5f sec interval.
    // also add to the list of GameObjects[].

    // effects can include: a new DialogueOptions appearing after speech from opposing party. Wait X seconds before automatically appearing?


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ActivateDialogue() {
        PlayerNullify.HardSetDisallow();
        Cursor.lockState = CursorLockMode.Locked;
    }
}
