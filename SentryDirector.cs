using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryDirector : MonoBehaviour {

    public List<GameObject> sentryList;
    public int[] typeList;
    public int[] movementList;
    //IdleMode SpinningMidLaserMode LightLookingDownMode SpinningLightMode SpinningMidScanMode MidScanMode ActivateTarget SeekingInfoScan

    private void Start() {
        StartCoroutine(DelayDirect());
    }

    private void Update() {

    }

    private void ActivateCommands() {
        for (int i = 0; i < sentryList.Count; i++) {
            SentryController sentryScript = sentryList[i].GetComponent<SentryController>();
            Check(sentryScript, typeList[i], movementList[i]);
        }
    }

    private void Check(SentryController sentryScript, int typeIndex, int movementIndex) {
        if (typeIndex == 0) sentryScript.IdleMode(movementIndex);
        else if (typeIndex == 1) sentryScript.SpinningMidLaserMode(movementIndex);
        else if (typeIndex == 2) sentryScript.LightLookingDownMode(movementIndex);
        else if (typeIndex == 3) sentryScript.SpinningLightMode(movementIndex);
        else if (typeIndex == 4) sentryScript.SpinningMidScanMode(movementIndex);
        else if (typeIndex == 5) sentryScript.MidScanMode(movementIndex);
        else if (typeIndex == 6) sentryScript.ActivateTarget(sentryScript.otherTarget);
        else if (typeIndex == 7) sentryScript.SeekingInfoScan(movementIndex);
    }

    private IEnumerator DelayDirect() {
        yield return new WaitForSeconds(1f);
        ActivateCommands();
    }

    public void DirectOneSentry(int sentryIndex, int typeIndex, int movementIndex) {
        Check(sentryList[sentryIndex].GetComponent<SentryController>(), typeIndex, movementIndex);
    }

}
