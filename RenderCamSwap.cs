using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCamSwap : MonoBehaviour {

    public GameObject[] ObjectList;
    private int currentIndex = 0;

    public void DisplayObject(int goalIndex) {
        ObjectList[currentIndex].SetActive(false);
        ObjectList[goalIndex].SetActive(true);
        currentIndex = goalIndex;
    }

}
