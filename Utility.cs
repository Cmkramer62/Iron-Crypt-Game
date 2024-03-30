using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Utility : MonoBehaviour{

    public AudioSource TwoDimAudioSource, HeldToolSource;
    public DoorRaycast raycastScript;

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

    #region ------------ COMM VARS
    public AudioSource commBeepingSource;
    public AudioClip beep2Clip;
    private bool takingCall = false;
    private bool awaitingPlayer = false;
    private CommActivator tempCommScript;
    #endregion

    public bool normUIEnabled = true;
    public bool allowedToUseItem = true;

    public GameObject player;

    private bool inAnimation = false;

    private UltimateCircularHealthBar circleBarsScript;

    void Start() {
        circleBarsScript = GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>();
    }

    // Update is called once per frame
    void Update() {
        if (allowedToUseItem) {
            if (Input.GetButtonDown("FKey") && hasFlashlight && player.GetComponent<Inventory>().heldIndex == 1) {// if we are holding and can use it.
                flashlightUsage();
            } else if (Input.GetButtonDown("FKey") && !hasFlashlight && player.GetComponent<Inventory>().heldIndex == 1) { // if we are holding flashlight but cannot use it.
                StartCoroutine(FlashlightSparks());
            }

            if (Input.GetButtonDown("FKey") && player.GetComponent<Inventory>().heldIndex == 0) {//Input.GetMouseButtonDown(1)
                cameraUsage();
            }

            if (Input.GetMouseButtonDown(0) && player.GetComponent<Inventory>().heldIndex == 2) { // if they are looking at it.
                //StartCoroutine(animTime(3, true));
                if (raycastScript.wrenching) {
                    wrenchAnimator.SetBool("wrenchingPlay", true);//Play("WrenchCrank");
                    HeldToolSource.loop = true;
                    HeldToolSource.Play();
                } else {
                    HelperText.PopupMessage("There's nothing to use this wrench on.", 4);
                }
               
            } else if (Input.GetMouseButtonUp(0) || !raycastScript.wrenching) {
                wrenchAnimator.SetBool("wrenchingPlay", false);
                HeldToolSource.loop = false;
            }

            if (!inAnimation && Input.GetMouseButtonDown(0) && player.GetComponent<Inventory>().heldIndex == 3) {
                StartCoroutine(animTime(1, false));
                axeAnimator.Play("AxeSwing");
                TwoDimAudioSource.PlayOneShot(swingCip);
            }

            if(Input.GetMouseButtonDown(0) && player.GetComponent<Inventory>().heldIndex == 4) {
                if (awaitingPlayer) CallActive(tempCommScript);
                else {
                    commBeepingSource.PlayOneShot(beep2Clip);
                    // call the player's MCVASpeaker. Method will display random voice line. "hello? you there?" "hey!...guess he can't hear me right now" 
                }
            }
        }
        if (normUIEnabled && GameObject.Find("Helper Systems").GetComponent<UIController>().GameIsPaused == false && 0 <= GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments && GameObject.Find("FlashCooldown").GetComponent<UltimateCircularHealthBar>().RemovedSegments <= 13f)
        {
            circleBarsScript.RemovedSegments -= Time.deltaTime * (10 / cameraCooldownTime) * 1.26f;
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

    private IEnumerator FlashlightSparks(){
        gameObject.GetComponent<MCVASpeaker>().PlayBrokenFlashlight();
        TwoDimAudioSource.PlayOneShot(flashlightBroken, Random.Range(.85f, 1.4f));
        flashlightPars.Play();
        yield return new WaitForSeconds(.05f);
        flashlightPars.Stop();
        
    }


    #endregion

    #region ------------- CAMERA

    private void cameraUsage() {
        if (cameraCooldown == false) {
            cameraCooldown = true;
            circleBarsScript.RemovedSegments = 13f;
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

    public void StartCommunication(CommActivator commActivator) {
        tempCommScript = commActivator;
        awaitingPlayer = true;
        StartCoroutine(CallWaiting(commActivator));
    }

    private IEnumerator CallWaiting(CommActivator commActivator) {
        commBeepingSource.Play();
        yield return new WaitForSeconds(15f);
        if(!takingCall) CallActive(commActivator);
    }

    private void CallActive(CommActivator commActivator) {
        commBeepingSource.Stop();
        commBeepingSource.PlayOneShot(beep2Clip);
        awaitingPlayer = false;
        takingCall = true;
        commActivator.PlaySequencially();
    }

    //communication:
    /*
     * Starts beeping periodically for maybe 30 seconds. The player can get to a safe place (closet, bathroom),
     * then scroll to it to activate the device. This will lock the player, no moving, they can look around still.
     * This will forcefully happen at the end of the 30 seconds.
     * Dialogue? otherwise simply play sounds. All on one gobjct with enables, each with diff initial waits. (0, 5, 14, 17, etc)
     * when done unlock player.
     */


}
