using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ScreenSet : MonoBehaviour {

    private GameObject player;
    public Transform screenLocation;
    public GameObject screenLookAt;
    private LookAtConstraint cameraLAC;
    public Animator lookAtAnimator;
    public Animator screenVisualAnimator;
    private GameObject NormalUI;

    public AudioSource screenSource;
    public AudioClip startClip;
    
    private bool lookingAtScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Two");
        cameraLAC = GameObject.Find("Main Camera").GetComponent<LookAtConstraint>();
        NormalUI = GameObject.Find("Normal UI");
    }

    // Update is called once per frame
    void Update()
    {
        //if (lookingAtScreen && Input.GetButtonDown("ooga")){
        //    exitScreen();
        //}
    }

    public void enterScreen() {
        screenSource.PlayOneShot(startClip);
        lookingAtScreen = true;
        // fade to black?
        // Teleport
        //Debug.Log("start");
        PlayerNullify.HardSetDisallow();//ChangePlayerAbilityPerms();
        player.transform.position = screenLocation.position;
        // fade in?
        // Set lookAt to ScreenLA
        // Set lookAt to active
        cameraLAC.constraintActive = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = screenLookAt.transform;
        source.weight = 1;
        cameraLAC.SetSource(0, source);

        

        NormalUI.SetActive(false);
        // Screen LA animation - Coroutine (wait for anim time)
        StartCoroutine(LookAtScreen());
        
        

    }

    IEnumerator LookAtScreen() {
        // after wait time, unlock mouse, disable ability to pause.
        lookAtAnimator.Play("ScreenAnim");
        screenVisualAnimator.Play("EnterScreen");
        yield return new WaitForSeconds(1);
        //GameObject.Find("Main Camera").GetComponent<DoorRaycast>().CrosshairChange(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void exitScreen() {
        StartCoroutine(LookAwayFromScreen());
    }

    IEnumerator LookAwayFromScreen() {
        lookAtAnimator.Play("ScreenAnim2");
        screenVisualAnimator.Play("EnterScreen2");
        cameraLAC.constraintActive = false;

        PlayerNullify.HardSetAllow();

        NormalUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(1);
        lookingAtScreen = false;
        HelperText.RemoveMessage();
        ObjectiveShower.ResumeText(10f);

    }

}
