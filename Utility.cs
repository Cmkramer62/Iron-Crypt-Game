using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Utility : MonoBehaviour{

    public AudioSource TwoDimAudioSource;

    #region -------------- FLASHLIGHT VARS

    public bool hasFlashlight = true; // Player will have flashlight by default.
    public GameObject flashlight;
    public bool flashlightActive = false;

    public AudioClip flashlightClipOn,flashlightClipOff, flashlightBroken;

    public VisualEffect flashlightPars;
    #endregion

    #region ------------- CAMERA VARS

    public bool hasCamera = false;
    public GameObject Camera;
    public bool cameraActive = false;

    public bool cameraCooldown = false;
    public float cameraCooldownTime = 3.2f;//determines cooldown of camera flash, in terms of seconds. Cannot be over ten.

    public AudioClip clickSound;
    public float timeLength = 0.02f;
    //public float timeLength = 0.045f;

    public bool alternateMon = false;
    #endregion

    #region ------------- AXE VARS
    public Animator axeAnimator;
    public AudioClip swingCip; // Play in Two Dim?
    #endregion

    #region ------------- WRENCH VARS
    public Animator wrenchAnimator;
    //public AudioClip swingCip; // Play in Two Dim?
    #endregion

    public bool normUIEnabled = true;
    public bool allowedToUseItem = true;

    public GameObject player;

    private bool inAnimation = false;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update() {
        if (allowedToUseItem) {
            if (Input.GetButtonDown("FKey") && hasFlashlight && player.GetComponent<Inventory>().heldIndex == 1) {// if we are holding and can use it.
                flashlightUsage();
            } else if (Input.GetButtonDown("FKey") && !hasFlashlight && player.GetComponent<Inventory>().heldIndex == 1) { // if we are holding flashlight but cannot use it.
                StartCoroutine(Flicker());
            }

            if (Input.GetButtonDown("FKey") && player.GetComponent<Inventory>().heldIndex == 0) {//Input.GetMouseButtonDown(1)
                cameraUsage();
            }

            if (!inAnimation && Input.GetMouseButtonDown(0) && player.GetComponent<Inventory>().heldIndex == 2) { // if they are looking at it.
                StartCoroutine(animTime(3, true));
                wrenchAnimator.Play("WrenchCrank");
                //TwoDimAudioSource.PlayOneShot(swingCip);
            }

            if (!inAnimation && Input.GetMouseButtonDown(0) && player.GetComponent<Inventory>().heldIndex == 4) {
                StartCoroutine(animTime(1, false));
                axeAnimator.Play("AxeSwing");
                TwoDimAudioSource.PlayOneShot(swingCip);
            }
        }
        if (normUIEnabled && GameObject.Find("Helper Systems").GetComponent<UIController>().GameIsPaused == false && 0 <= GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments && GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments <= 13f)
        {
            GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments -= Time.deltaTime * (10 / cameraCooldownTime) * 1.26f;
        }
    }

    #region ------------- FLASHLIGHT


    public void flashlightUsage() {
        if (!flashlightActive) {
            TwoDimAudioSource.PlayOneShot(flashlightClipOn);
            flashlight.SetActive(true);
            flashlightActive = true;
        }
        else {
            TwoDimAudioSource.PlayOneShot(flashlightClipOff);
            flashlight.SetActive(false);
            flashlightActive = false;
        }
    }

    private IEnumerator Flicker(){
        TwoDimAudioSource.PlayOneShot(flashlightBroken, Random.Range(.85f, 1.4f));
        flashlightPars.Play();
        yield return new WaitForSeconds(.05f);
        flashlightPars.Stop();
        
    }


    #endregion

    #region ------------- CAMERA

    private void cameraUsage() {
        if (cameraCooldown == false) {
            Debug.Log("cooldown");
            cameraCooldown = true;
            GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments = 13f;
            if (alternateMon) { GameObject.Find("Helper Systems").GetComponent<AlternateCrawl>().alternate(); }
            TwoDimAudioSource.PlayOneShot(clickSound);
            StartCoroutine(lightUp());
            StartCoroutine(cooldownTimer());
        }
    }
        
    /*Activates the flash for the Camera. Interval between-
    /duration of each light in flash is based on timeLength.
    */
    IEnumerator lightUp() {
        cameraActive = true;
        Camera.SetActive(true);
        yield return new WaitForSeconds(timeLength);
        Camera.SetActive(false);

        yield return new WaitForSeconds(0.045f);
        Camera.SetActive(true);

        yield return new WaitForSeconds(timeLength);
        Camera.SetActive(false);


        cameraActive = false;

    }

    //Cooldown for the camera flash.
    IEnumerator cooldownTimer(){
        yield return new WaitForSeconds(cameraCooldownTime);
        cameraCooldown = false;
    }

    #endregion

    private IEnumerator animTime(int duration, bool restricted) {
        inAnimation = true;
        if (restricted) {
            player.GetComponent<PlayerMovement2>().allowedToMove = false;
            GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = false;
        }
        yield return new WaitForSeconds(duration);
        if (restricted) {
            player.GetComponent<PlayerMovement2>().allowedToMove = true;
            GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = true;
        }
        inAnimation = false;
    }


}
