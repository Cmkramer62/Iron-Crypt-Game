using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryArmActivator : MonoBehaviour {

    private SentryController sentryScript;

    // Start is called before the first frame update
    void Start() {
        sentryScript = gameObject.transform.GetComponentInParent<SentryController>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("InteractiveSwitch") ){
            StartCoroutine(ActivateTimer(other));
        }
    }

    private IEnumerator ActivateTimer(Collider other) {

        sentryScript.MovementChanger(0);
        sentryScript.armAnimator.Play("Extend");
        yield return new WaitForSeconds(3f);
        other.GetComponent<Activator>().ForceActivateSwitch();
        sentryScript.armAnimator.Play("Retract");
        yield return new WaitForSeconds(1f);
        gameObject.transform.parent.gameObject.SetActive(false);

        sentryScript.IdleMode(1);
    }
}
