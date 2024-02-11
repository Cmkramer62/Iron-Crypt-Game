using UnityEngine;
using UnityEngine.Animations;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ItemGrabDel : MonoBehaviour {

    public GameObject star;
    public AudioClip grabClip;
    public bool note, showObjective = false;
    public string objectiveText;

    private Transform target;
    private bool move = false;
    private float t = 0.2f;

    // Start is called before the first frame update
    void Start() {
        if (!note) target = GameObject.Find("itemTarget").transform; // Has this been renamed?
        else target = GameObject.Find("noteTarget").transform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (move) {
            Vector3 a = gameObject.transform.position;
            Vector3 b = target.position;
            transform.position = Vector3.Lerp(a, b, t);
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            //gameObject.SetActive(false);
        }
    }

    public void ActivateGrabDelete() {
        move = true;
        ShowObjective();
        GameObject.Find("--- 2D QUIET SOURCE ---").GetComponent<AudioSource>().PlayOneShot(grabClip);
    }


    public void ShowObjective() {
        if (showObjective) {
            ObjectiveShower.ShowTextTimed(objectiveText, 10f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!note && move && other.name.Equals("itemTarget")){
            Debug.Log("fesuf");
            gameObject.SetActive(false);
        } else if(note && move && other.name.Equals("noteTarget")) {
            Debug.Log("note");
            gameObject.GetComponent<Activator>().ForceActivateSwitch();
        }
    }

}
