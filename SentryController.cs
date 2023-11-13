using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;

public class SentryController : MonoBehaviour {

    private NavMeshAgent sentryNMA;
    public Transform defaultTarget;
    private Transform originalTarget;

    public GameObject laser, spotlight, scanbeam, scanbeamHitbox, infoscan, walkieBody, projection, arm;
    public AudioSource sentryAudio;
    public AudioClip alarmClip;
    public bool followConstantly = false;

    public bool spinConstantly = false;
    public float spinIntensity = 10f;

    public float sentryModeHeight = 9f;

    private bool moveDown, moveUp = false;

    public AlarmActivator AlarmScript;
    private bool alarmActive = false;

    public Animator armAnimator;

    public GameObject otherTarget;

    public float speed = 5f; // THIS OVERWRITES THE NMA speed setting!

    private Coroutine alarmRoutine;

    void Start() {
        sentryNMA = gameObject.GetComponent<NavMeshAgent>();
        originalTarget = defaultTarget;
        if (spinConstantly) sentryNMA.updateRotation = false;
        sentryNMA.speed = speed;
    }

    void Update() {
        if(followConstantly) sentryNMA.SetDestination(defaultTarget.position);
 
        if(spinConstantly) gameObject.transform.Rotate(0, spinIntensity * Time.deltaTime, 0);

        if(moveUp) {
            if (sentryNMA.baseOffset >= sentryModeHeight) moveUp = false;
            else sentryNMA.baseOffset += 5f * Time.deltaTime;
        } else if (moveDown) {
            if (sentryNMA.baseOffset <= 5) moveDown = false;
            else sentryNMA.baseOffset -= 5f * Time.deltaTime;
        }
    }

    #region-------------------MOVEMENT----------------------

    /*
     * MovementChanger() will call helper methods to change 
     * the movement of the sentry (ex. idle, following, patrolling)
     */
    public void MovementChanger(int movementType) {
        if(movementType == 0) {
            StopMoving();
        } else if(movementType == 1) {
            UnfollowPlayer();
            sentryNMA.speed = speed;
            followConstantly = true;
        } else if(movementType == 2) {
            FollowPlayer();
        }
    }

    private void StopMoving() {
        sentryNMA.speed = 0;
        followConstantly = false;
    }

    private void SetStaticTarget() {
        sentryNMA.speed = speed;
        sentryNMA.SetDestination(defaultTarget.position);
    }

    private void SetConstantTarget() {
        sentryNMA.speed = speed;
        followConstantly = !followConstantly;
    }

    private void OverrideTargetConst(Transform newTarget) {
        sentryNMA.speed = speed;
        originalTarget = defaultTarget;
        defaultTarget = newTarget;
        if (!followConstantly) SetConstantTarget();
    }

    private void FollowPlayer() {
        followConstantly = true;

        sentryNMA.speed = speed;
        OverrideTargetConst(GameObject.Find("Player Two").transform);
        SetSpin(0, 0);
        sentryNMA.stoppingDistance = 10f;
    }

    private void GoToTarget(GameObject target) {
        sentryNMA.speed = speed;
        OverrideTargetConst(target.transform);
        SetSpin(0, 0);
        sentryNMA.stoppingDistance = 2f;
    }

    private void UnfollowPlayer() {
        defaultTarget = originalTarget;
    }

    public void SetStoppingDistance(float distance) {
        sentryNMA.stoppingDistance = distance;
    }
    #endregion

    #region -------------------POSITION----------------------
    /*
     * 3 Main sub-aspects to POSITION:
     * 1. Spin
     * 2. Height (base height)
     * 3. Looking Up or Down
     */
    /*
      * NOTE: float degreesASecond & float duration is unused if the sentry is spinning.
      * SetSpin() will activate sentry spinning if it's not already, otherwise it will stop spinning if it is already.
      * If sentry is being set to spinning, degreesASecond is set & duration is used if able.
      * Additionally, SetSpin() will disable navmeshagent.UpdateRotation. (Two don't work together).
      */
    private void SetSpin(float degreesASecond, float duration = 0f) {
        if(degreesASecond > 0) {
            spinConstantly = true;
            sentryNMA.updateRotation = false;
            if (spinConstantly) {
                spinIntensity = degreesASecond;
                if (duration > 0f) {
                    StartCoroutine(SpinForLimitedTime(duration));
                }
            }
        } else {
            spinConstantly = false;
            sentryNMA.updateRotation = true;
        }
    }

    private IEnumerator SpinForLimitedTime(float duration) {
        yield return new WaitForSeconds(duration);
        spinConstantly = false;
    }

    private void GoUp() {
        moveUp = true;
        moveDown = false;
    }

    private void GoDown() {
        moveUp = false;
        moveDown = true;
    }

    private void LookUp() {
        // if not already looking up
        walkieBody.GetComponent<Animator>().Play("lookSentryUp");
    }

    private void LookDown() {
        // if not already looking down
        walkieBody.GetComponent<Animator>().Play("lookSentryDown");
    }
    #endregion

    #region-------------------WEAPONS-----------------------
   
    /*
     * Turns every single weapon of the sentry off.
     */
    private void DisableAll() {
        ChangeWeapons(false, false, false, false);
    }

    /*
     * Laser simply damages.
     */
    private void ActivateLaser() {
        ChangeWeapons(true, false, false, false);
    }

    /*
     * Spotlight will allow it to see you if entered.
     */
    private void ActivateLight() {
        ChangeWeapons(false, true, false, false);
    }

    /*
     * Scanning Laser only detects movement?
     */
    private void ActivateScan() {
        ChangeWeapons(false, false, true, false);
        StartCoroutine(ScanMercyTimer());
    }

    private IEnumerator ScanMercyTimer() {
        yield return new WaitForSeconds(1f);
        scanbeamHitbox.SetActive(true);
    }

    /*
     * Scanning Laser only for nonfunctional (Scanning data).
     */
    private void ActivateInfoScan() {
        ChangeWeapons(false, false, false, true);
    }

    /*
     * Enables and Disables the corresponding weapon GameObjects.
     */
    private void ChangeWeapons(bool laserOn, bool spotOn, bool scanOn, bool infoOn) {
        laser.SetActive(laserOn);
        spotlight.SetActive(spotOn);
        scanbeam.SetActive(scanOn);
        scanbeamHitbox.SetActive(!scanOn);
        infoscan.SetActive(infoOn);
    }

    #endregion

    /*
     * A MODE is a preset combination of [position] and [weapons]
     */
    #region-------------------MODE--------------------------

    private void SetMode(int movementType, bool alarm, int weaponType, bool goUp, bool lookUp, bool spin) {
        MovementChanger(movementType);

        alarmActive = alarm;

        if (weaponType == 0) DisableAll();
        else if (weaponType == 1) ActivateLaser();
        else if (weaponType == 2) ActivateLight();
        else if (weaponType == 3) ActivateScan();
        else if (weaponType == 4) ActivateInfoScan();

        if (goUp) GoUp(); else GoDown();
        if (lookUp) LookUp(); else LookDown();

        if (spin) SetSpin(90f, 0f);
        else  SetSpin(0f, 0f); 
    }


    // not moving, moving towards target, move towards player
    public void IdleMode(int movementType) {
        SetMode(movementType, false, 0, false, true, false);
    }

    public void SpinningMidLaserMode(int movementType) {
        SetMode(movementType, false, 1, false, true, true);
    }

    public void LightLookingDownMode(int movementType) {
        SetMode(movementType, false, 2, true, false, false);
    }

    public void SpinningLightMode(int movementType) {
        SetMode(movementType, false, 2, false, true, true);
    }

    public void SpinningMidScanMode(int movementType) {
        SetMode(movementType, false, 3, false, true, true);
    }

    public void MidScanMode(int movementType) {
        SetMode(movementType, false, 3, false, true, false);
    }

    public void ActivateTarget(GameObject target) {
        MovementChanger(1);
        GoToTarget(target);
        arm.SetActive(true);
    }

    public void SeekingInfoScan(int movementType) {
        SetMode(movementType, false, 4, true, true, false);
    }

    #endregion

    #region ----------------------EFFECTS-----------------------
    
    /*
     * ActivateEffect() turns on an effect that is independant from the sentry.
     * An example could include a building alarm system being turned on.
     */
    public void ActivateEffect(int effectType) {
        if (effectType == 0) {
            AlarmTripGlobal();
        } else if(effectType == 1) {
            AlarmDisableGlobal();
        } else if(effectType == 2) {
            AlarmTripLocal();
        } else if(effectType == 3) {
            StartCoroutine(Projection());
        }
    }

    public void ClearEffects() {
        if(alarmActive) StopCoroutine(alarmRoutine);
        sentryAudio.Stop();
        alarmActive = false;
        AlarmDisableGlobal();
    }

    /*
     * AlarmTripGlobal() activates the building alarm system.
     */
    private void AlarmTripGlobal() {
        AlarmScript.ManualAlarmOn();
    }

    /*
     * AlarmDisableGlobal() disables the building alarm system.
     */
    private void AlarmDisableGlobal() {
        AlarmScript.ManualAlarmOff();
    }

    /*
     * AlarmTripLocal() turns the sentry into a local alarm.
     * This is not an independant effect, as this overrides
     * and current weapons, modes, movement, etc.
     */
    private void AlarmTripLocal() {
        SpinningLightMode(0);
        sentryAudio.PlayOneShot(alarmClip);
        spinIntensity *= 3;
        alarmActive = true;
        alarmRoutine = StartCoroutine(LocalAlarmTimer());
        if(GameObject.Find("PLAYER FLASH")) GameObject.Find("PLAYER FLASH").transform.position = gameObject.transform.position;
        //color yellow
    }

    /*
     * Projection() activates a hologram that is projected
     * underneath the Sentry.
     */
    private IEnumerator Projection() {
        LightLookingDownMode(0);
        yield return new WaitForSeconds(1f);
        projection.SetActive(true);
        spotlight.GetComponentInChildren<HDAdditionalLightData>().color = Color.blue;
    }
    #endregion

    private IEnumerator LocalAlarmTimer() {
        yield return new WaitForSeconds(.1f);
        spotlight.SetActive(false);
        yield return new WaitForSeconds(.1f);
        spotlight.SetActive(true);
        if(alarmActive) StartCoroutine(LocalAlarmTimer());
    }
}
