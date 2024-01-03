using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueOptions : MonoBehaviour {
        
    public string[] listOfDialogue;
    public List<GameObject> boxesList;
    public GameObject gridParent, boxPrefab;

    public bool enterTrigger = false;
    private bool done = false;

    // list of strings. "yes", "no", "Im not sure";
    // on Activation()-> nullify the player. bring out mouse. disable ability to pause. 
    // for each string in list, total int. GameObject.Instantiate(prefab for boxes) as a child of the grid. fade in one at a time, .5f sec interval.
    // also add to the list of GameObjects[].

    // effects can include: a new DialogueOptions appearing after speech from opposing party. Wait X seconds before automatically appearing?


    // Start is called before the first frame update
    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && enterTrigger) StartActivateDialogue();
    }

    public void StartActivateDialogue() {
        if (!done) {
            done = true;
            StartCoroutine(ActivateDialogue());
        }
    }

    public IEnumerator ActivateDialogue() {
        boxesList = new List<GameObject>();

        PlayerNullify.HardSetDisallow();
        //PlayerNullify.PlayerAbilitiesSetMovementException(true);
        Cursor.lockState = CursorLockMode.None;
        GameObject.Find("Helper Systems").GetComponent<UIController>().canPause = false;

        int total = listOfDialogue.Length;
        
        for (int i = 0; i < total; i++) {
            GameObject newThing = GameObject.Instantiate(boxPrefab);
            newThing.transform.parent = gridParent.transform;
            newThing.GetComponentInChildren<TextMeshProUGUI>().text = listOfDialogue[i];
            newThing.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            boxesList.Add(newThing);
            newThing.GetComponent<DialogueEffects>().parentOptionsScript = gameObject.GetComponent<DialogueOptions>();

            newThing.GetComponentInChildren<Animator>().Play("DialogueSlideIn");
            yield return new WaitForSeconds(.25f);
        }
        
    }

    public void StartDeactivateDialogue() {
        StartCoroutine(DeactivateDialogue());
    }

    public IEnumerator DeactivateDialogue() {

        PlayerNullify.HardSetAllow();
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Helper Systems").GetComponent<UIController>().canPause = true;

        for (int i = boxesList.Count-1; i >= 0; i--) {
            boxesList[i].GetComponentInChildren<Animator>().Play("DialogueSlideOut");
            yield return new WaitForSeconds(.25f);
            //GameObject.Destroy(boxesList[i]);
            StartCoroutine(DeactivationBuffer(boxesList[i]));
        }
        
    }

    private IEnumerator DeactivationBuffer(GameObject box) {
        yield return new WaitForSeconds(.75f);
        GameObject.Destroy(box);
    }

}
