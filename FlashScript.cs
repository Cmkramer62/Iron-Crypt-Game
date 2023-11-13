using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;
using RengeGames.HealthBars;

public class FlashScript : MonoBehaviour
{

    public AudioClip clip;
    public AudioClip alertClip;
    public bool isOn = true;
	public GameObject lightSource;
    public GameObject flashLight;
    private bool flashLightOn = true;

    #region AUDIO_VARS
    public AudioSource alertSource;
	public AudioSource clickSound;
	public AudioSource chaseMusic;
    public AudioSource chaseStepsSound;
	public float timeDelay = 0.02f;

    public float flCooldown = 3.2f;//determines cooldown of camera flash, in terms of seconds. Cannot be over ten.

    private bool isOnCooldown = false;
     
    public AudioClip flashLightON;
    public AudioClip flashLightOff;
    #endregion

    public Transform teleportTarget;
	public GameObject playerFlash;

	public static bool isChasing = false;

    //.private int flashCounter = 0;
    public GameObject script;

    // Update is called once per frame
    void Update() {

        if(GameObject.Find("Player Two").GetComponent<PlayerMovement2>().unreachable == true) {
            stopChasing(); //THIS IS NOT GOOD
        }
        else {
            if ((GameObject.Find("Ghoul").GetComponent<enemyController>().withinRange == true)) {
                if (flashLightOn) {
                    //once it spots you, the within red radius is as big as the normalFlashRadius. the red radius shrinks (over five seconds) to its normal radius.
                    //(so even if you turn your flash light off instantly, and you get out of the area, it will still chase you for a bit).
                    if (isChasing == false){
                        alertSource.PlayOneShot(alertClip);
                        Debug.Log("Do the R O A R");
                    }
                    chaseFunc();
                }
            }
            if ((GameObject.Find("Ghoul").GetComponent<enemyController>().withinRed2 == true)) {
                chaseFunc();
            }
        }

        #region FLASHLIGHT
        if (Input.GetButtonDown("FKey")) {
            if (!flashLightOn) {
                clickSound.PlayOneShot(flashLightON);
                flashLight.SetActive(true);
                flashLightOn = true;
            }
            else {
                clickSound.PlayOneShot(flashLightOff);
                flashLight.SetActive(false);
                flashLightOn = false;
            }
        }
        #endregion

        if (GameObject.Find("UI CANVAS").GetComponent<UIController>().GameIsPaused == false &&  0 <= GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments && GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments <= 13f) {
            GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments -= Time.deltaTime * (10/flCooldown) * 1.26f;
        }

        if (Input.GetMouseButtonDown(1)) {
            if (isOnCooldown == false){//!clickSound.isPlaying)  {
                isOnCooldown = true;
                GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar> ().RemovedSegments = 13f;
                clickSound.PlayOneShot(clip);
                StartCoroutine(lightUp());
                StartCoroutine(cooldownTimer());
            }
            /*
            if (GameObject.Find("Ghoul").GetComponent<enemyController>().withinRange == true)// Mon goes to flash
            {
                //chaseFunc();
            }
            */
        }
        /*
        if (Input.GetKey(KeyCode.LeftShift)) {//Mon follows sprint
            if (GameObject.Find("Ghoul").GetComponent<enemyController>().withinRange == true)
            {
                //chaseFunc();
            }
        }
        */

        if (isChasing == true && !chaseMusic.isPlaying) {
			chaseMusic.Play();
            chaseStepsSound.Play();
		}
		
	}

	void OnTriggerEnter(Collider other){ //When enemy touches flash's last location:
		if (other.gameObject.name == "dontrename") {
            stopChasing();
        }
	}

    private void stopChasing() { // we want to call this when we figure out how to tell when the flash thing is in an unreachable space.
        isChasing = false;
        chaseMusic.Stop();
        chaseStepsSound.Stop();
        GameObject.Find("Ghoul").GetComponent<enemyController>().hunting = true;
       
    }

    public void chaseFunc() {
        isChasing = true;
        GameObject.Find("Ghoul").GetComponent<enemyController>().hunting = false;
        playerFlash.transform.position = teleportTarget.transform.position;
        isChasing = true;
        
    }

	IEnumerator lightUp() {//Activates the flash for the Camera. Interval between-
        //two flashes is based on float timeDelay.
		isOn = true;
		lightSource.SetActive(true);
		//clickSound.Play();
		yield return new WaitForSeconds(timeDelay);
		lightSource.SetActive(false);

        yield return new WaitForSeconds(0.045f);
        lightSource.SetActive(true);
        //clickSound.Play();
        yield return new WaitForSeconds(timeDelay);
        lightSource.SetActive(false);


        isOn = false;

    }

    IEnumerator cooldownTimer() {//Cooldown for the camera flash.
        yield return new WaitForSeconds(flCooldown);
        isOnCooldown = false;
    }

}

