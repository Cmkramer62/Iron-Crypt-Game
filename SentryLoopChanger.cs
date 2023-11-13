using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryLoopChanger : MonoBehaviour {

    public bool ableToLoop = true;

    public SentryDirector directorScript;
    //public bool looping = false;
    public int[] sentryIndecies;
    public int typeOfNewSetting;
    public int newMovement;
    public float durationMin, durationMax;
    private float duration;

    public bool collisionActivate, doOnce = true;
    private bool done = false;
    public bool looping = true;
    private bool gaveNewEffects = true;

    private List<int> typeList2;
    private List<int> movementList2;


    private void OnTriggerEnter(Collider other) {
        if (other.name.Equals("Player Two") && collisionActivate && (!done || !doOnce)) {
            if (doOnce) done = true;

            if (looping) StartCoroutine(ActivateLoopTimer());
            else if (gaveNewEffects) StartLoop();
            else EndLoop();
        }
    }

    private IEnumerator ActivateLoopTimer() {
        if (ableToLoop) StartLoop();
        yield return new WaitForSeconds(Random.Range(durationMin, durationMax) / 2);
        if (ableToLoop) EndLoop();
        yield return new WaitForSeconds(Random.Range(durationMin, durationMax) / 2);
        if(ableToLoop) StartCoroutine(ActivateLoopTimer());
    }


    private void StartLoop() {
        gaveNewEffects = false;
        typeList2 = new List<int>();
        movementList2 = new List<int>();

        foreach (int index in sentryIndecies) {
            typeList2.Add(directorScript.typeList[index]);
            movementList2.Add(directorScript.movementList[index]);

            directorScript.DirectOneSentry(index, typeOfNewSetting, newMovement);
        }
    }

    private void EndLoop() {
        gaveNewEffects = true;
        int i = 0;
        foreach (int index in sentryIndecies) {
            directorScript.DirectOneSentry((index), typeList2[i], movementList2[i]);
            i++;
        }
    }

}
