using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerMovement2 : MonoBehaviour
{
    public GroundChecker groundCheckerScript;


    #region MOVEMENT VARIABLES

    public bool allowedToMove = true;

    public CharacterController controller;
    public float speed = 12f;
    private float OGspeed;
    private float HFspeed;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float pitchValue = 1.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool shouldBeSlowed = false;
    public bool isSprinting = false;
    float sprintMultiplier = 1f;

    private int indexStepSound;

    // Crosshair
    public bool lockCursor = true;

    // Internal Variables
    private Image crosshairObject;

    public float sprintDuration = 5f;
    // Sprint Bar
    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;
    public float sprintRecoveryDec = 1f;//1 = 100%, .9 = 90%

    // Internal Variables
    private CanvasGroup sprintBarCG;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    //tired
    private bool isTired = false;

    //private GrayscaleCamera grayScript;
    #endregion

    #region HEADBOB VARIABLES
    private Vector3 objectOrigin;
    public Transform objectParent;
    private float idleCounter;
    private float normalCounter;
    private Vector3 objectBobPosition;
    //---------------------
    public Transform objectParent2;
    private Vector3 objectOrigin2;
    private float idleCounter2;
    private float normalCounter2;
    private Vector3 objectBobPosition2;
    #endregion

    #region SOUND VARIABLES
    [SerializeField] AudioClip[] footstepClips;
    private bool stepped = false;
    private bool isIdle = false;

    public AudioClip breathClip;

    public AudioSource footstep;
    public AudioSource twoDSource;
    //public AudioClip footstepClip;
    public AudioClip ventstepClip;
    public AudioClip crouchClip;
    [SerializeField] AudioClip[] jumpClip;

    public bool isInVent = false;
    #endregion

    #region CROUCH VARIABLES
    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public bool isCrouched = false;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float crouchHeight = .75f;
    private float temp = .75f;
    public float speedReduction = .5f;
    private Vector3 originalScale;

    //public float walkSpeed = 5f;
    private float goal;
    #endregion

    #region VENT VARIABLES
    private bool ened = false;
    public int amVen = 0;
    public bool unreachable = false;
    #endregion

    private void Awake()
    {
        sprintRemaining = sprintDuration;
        originalScale = transform.localScale;
        goal = originalScale.y;
    }

    private void Start()
    {
        //grayScript = GameObject.Find("Main Camera").GetComponent<GrayscaleCamera>();
        objectOrigin = objectParent.localPosition;
        objectOrigin2 = objectParent2.localPosition;
        OGspeed = speed;
        HFspeed = speed / 2;
        //Debug.Log(speed +" " + OGspeed);
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }
        #region Sprint Bar

        sprintBarCG = GameObject.Find("SprintBar").GetComponentInChildren<CanvasGroup>();

        if (useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(true);
            sprintBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            sprintBarWidth = screenWidth * sprintBarWidthPercent;
            sprintBarHeight = screenHeight * sprintBarHeightPercent;

            sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
            sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

            if (hideBarWhenFull)
            {
                sprintBarCG.alpha = 0;
            }
        }
        else
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
        }

        #endregion
    }

    #region PRIVATE METHODS


    private void HeadBob(float p_z, float p_x_intensity, float p_y_intesity) {//Headbob for Camera
        objectBobPosition = objectOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intesity, 0);
        if (0 >= (Mathf.Sin(p_z * 2) * p_y_intesity) && !stepped)
        {
            stepped = true;
            if (groundCheckerScript.isGrounded && !isIdle) {
                groundCheckerScript.PlaySound();
            }

        }
        else if (0 < (Mathf.Sin(p_z * 2) * p_y_intesity))
        {
            stepped = false;
        }

    }

    private void HeadBob2(float p_z, float p_x_intensity, float p_y_intesity)//Headbob for photography handheld Camera
    {
        objectBobPosition2 = objectOrigin2 + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intesity, 0);

    }

    public void Crouch()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if (isCrouched)
        {
            temp = crouchHeight;
            //transform.localScale = new Vector3(originalScale.x, Mathf.Clamp(temp += 10 * Time.deltaTime, crouchHeight, originalScale.y), originalScale.z);//originalScale.y, originalScale.z);
            goal = originalScale.y;

            speed = OGspeed;

            isCrouched = false;
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            twoDSource.PlayOneShot(crouchClip);
            temp = originalScale.y;
            //transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            goal = crouchHeight;
            speed = HFspeed;

            isCrouched = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ANTI-CROUCH")
        {
            amVen++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ANTI-CROUCH")
        {
            amVen--;
        }
    }

    #endregion


    void Update()
    {

        //if (isTired && grayScript.WeightGetter() < 1) grayScript.ChangeValue(1f, Time.deltaTime);//grayScript.intensity += 1f * Time.deltaTime;
        //else if (!isTired && grayScript.WeightGetter() > 0) grayScript.ChangeValue(-.5f, Time.deltaTime); // grayScript.intensity -= .5f * Time.deltaTime;

        #region MOVEMENT

        Vector3 inputVector = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        if(inputVector.magnitude > 1) {
            inputVector.Normalize();
        }

        if (allowedToMove) {
            controller.Move(inputVector * sprintMultiplier * speed * Time.deltaTime); 
        }
        

        #endregion

        #region SPRINT/SPRINT UI
        if (isSprinting && !isTired)
        {
            sprintRemaining -= 1 * Time.deltaTime;
            if (hideBarWhenFull) { sprintBarCG.alpha += 5 * Time.deltaTime; }
        }
        else
        {
            sprintRemaining = Mathf.Clamp(sprintRemaining += sprintRecoveryDec * Time.deltaTime, 0, sprintDuration);
        }

        float sprintRemainingPercent = sprintRemaining / sprintDuration;
        //Debug.Log(sprintRemainingPercent);
        sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);

        if (sprintRemaining <= 0)
        {
            isTired = true;
            //color change
            sprintBar.color = Color.red;
            twoDSource.PlayOneShot(breathClip);
        }
        if (sprintRemaining == sprintDuration)
        {
            isTired = false;
            if (hideBarWhenFull) { sprintBarCG.alpha -= 3 * Time.deltaTime; }
            sprintBar.color = Color.white;
        }
        //Debug.Log(isTired);

        if ((Input.GetKey(KeyCode.W)) && groundCheckerScript.isGrounded && Input.GetKey(KeyCode.LeftShift) && useSprintBar && !isTired && allowedToMove)
        {
            isSprinting = true;
            sprintMultiplier = 2;
        }
        else if ( (isTired || groundCheckerScript.isGrounded) || (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.LeftShift))) // or is Grounded (we don't want to disable sprinting 
        {
            isSprinting = false;
            sprintMultiplier = 1;
        }
        #endregion

        #region Crouch
        //to prevent uncrouching in areas not supposed to, make invisible collider in those areas, which sets the
        //variable enableCrouch to false. Problem is: would that make FPC uncrouch when that happens?

        //Debug.Log(temp);
        if (isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, Mathf.Clamp(temp -= 2f * Time.deltaTime, crouchHeight, originalScale.y), originalScale.z);

            //transform.localScale = new Vector3(originalScale.x, temp -= 2 * Time.deltaTime, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, Mathf.Clamp(temp += 2f * Time.deltaTime, crouchHeight, originalScale.y), originalScale.z);
            //transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        if (enableCrouch && allowedToMove)
        {
            if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
            {
                Crouch();
            }

            if (Input.GetKeyDown(crouchKey) && holdToCrouch)
            {
                isCrouched = false;
                Crouch();
            }
            else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
            {
                isCrouched = true;
                Crouch();
            }
        }

        #endregion

        #region JUMPING
        if (groundCheckerScript.isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        if (Input.GetButtonDown("Jump") && groundCheckerScript.isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            twoDSource.PlayOneShot(jumpClip[Random.Range(0, jumpClip.Length)]);
        }

        velocity.y += gravity * Time.deltaTime;
        if(allowedToMove)controller.Move(velocity * Time.deltaTime);
        #endregion

        #region HEADBOB
        
        objectParent.localPosition = Vector3.Lerp(objectParent.localPosition, objectBobPosition, Time.deltaTime * 8f);
        if (isSprinting && allowedToMove)
        {
            HeadBob(normalCounter, .4f, .4f);//0.399993
            normalCounter += (Time.deltaTime * 8.8f);
            isIdle = false;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !isSprinting && allowedToMove)
        {
            HeadBob(idleCounter, .1f, .1f);//0.09999994
            idleCounter += (Time.deltaTime * 6f);
            isIdle = false;
        }
        else if (allowedToMove)
        {
            isIdle = true;
            HeadBob(idleCounter, .025f, .025f);//0.02499999
            idleCounter += (Time.deltaTime * 0.6f);
        }
        
        #endregion

        #region HEADBOB2
        objectParent2.localPosition = Vector3.Lerp(objectParent2.localPosition, objectBobPosition2, Time.deltaTime * 8f);
        if (isSprinting && allowedToMove)
        {
            HeadBob2(normalCounter2, .025f, .025f);
            normalCounter2 += (Time.deltaTime * 8.8f);

        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !isSprinting && allowedToMove)
        {
            HeadBob2(idleCounter2, .025f, .025f);
            idleCounter2 += (Time.deltaTime * 6f);

        }
        else if (allowedToMove)
        {

            HeadBob2(idleCounter2, .025f, .025f);
            idleCounter2 += (Time.deltaTime * 0.6f);
        }
        #endregion
    }

    
    
}
