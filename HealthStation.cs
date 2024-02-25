using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStation : MonoBehaviour {

    public Animator ArmAnimator;
    public AudioSource ArmSource;
    public AudioClip TwinkleClip;
    public GameObject forceField;

    public bool canBeBroken = true;

    private bool onCooldown = false;
    //OnStart, roll 50/50 chance for broken


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActivateMachine() {
        if (!onCooldown) {
            forceField.SetActive(true);
            ArmAnimator.Play("HealthStationArmAnim");
            ArmSource.Play();
            StartCoroutine(AnimTimer());
        }
    }

    private IEnumerator AnimTimer() {
        onCooldown = true;
        yield return new WaitForSeconds(10f);
        ArmSource.Stop();
        forceField.SetActive(false);
        gameObject.GetComponent<AudioSource>().PlayOneShot(TwinkleClip, .2f);
        GameObject.Find("Player Two").GetComponent<HealthSystem>().RaiseLife();
        GameObject.Find("Player Two").GetComponent<HealthSystem>().RaiseLife();
        HelperText.PopupMessage("Health Restored.", 5);
        onCooldown = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) ActivateMachine();
    }

}
