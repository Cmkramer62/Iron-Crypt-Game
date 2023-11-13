using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AlternateCrawl : MonoBehaviour {
    private int mon = 1;
    public GameObject monsterOne, monsterTwo, monsterThree;
    public PositionConstraint pivotOne, pivotTwo, pivotThree;

    public Transform target;
    public Transform transportee;

    public Transform ghoul;


    public void Update() {
        //if player isSprinting == true, set Crawler.navmeshagent.speed += .1;
        // (this should effectively increase the crawler's speed by .1 for every single
        // frame that the player is sprinting.)

            //inner if: if counter true: set counter to false, start coroutine where mon chases player for 
            //5 seconds minimum. at end, set counter to true.
    }

    public void alternate() {
        if (mon == 1) {
            mon = 2;
            StartCoroutine(flashWaitTimer(false, true, false));
        }
        else if (mon == 2 ) {
            mon = 3;
            StartCoroutine(flashWaitTimer(false, false, true));
        }
        else if (mon == 3 ) {
            mon = 1;
            StartCoroutine(flashWaitTimer(true, false, false));
        }
    }


    IEnumerator flashWaitTimer(bool one, bool two, bool three){
        //GameObject.Find("FakeCrawlers").GetComponent<FlashTarget>().teleport();
        transportee.position = target.position;

        monsterOne.SetActive(one);
        monsterOne.GetComponent<monRanSound>().allowed = one;
        //pivotOne.constraintActive = !one;
        

        monsterTwo.SetActive(two);
        monsterTwo.GetComponent<monRanSound>().allowed = two;
        //pivotTwo.constraintActive = !two;
        

        monsterThree.SetActive(three);
        monsterThree.GetComponent<monRanSound>().allowed = three;
        //pivotThree.constraintActive = !three;
        

        yield return new WaitForSeconds(.5f);
        monsterOne.SetActive(false);
        if (one) { ghoul.position = monsterOne.transform.position; }
        monsterTwo.SetActive(false);
        if (two) { ghoul.position = monsterTwo.transform.position; }
        monsterThree.SetActive(false);
        if (three) { ghoul.position = monsterThree.transform.position; }

    }
}
