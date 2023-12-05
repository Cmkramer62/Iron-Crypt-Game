using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexDisplay : MonoBehaviour {

    public RenderCamSwap renderModelScript;

    public bool dataAchieved = false;
    public string informationText = "";
    public GameObject dataVisuals;
    public int renderListIndex = 0;

    public void DisplayModel() {
        if (dataAchieved) renderModelScript.DisplayObject(renderListIndex);
        else renderModelScript.DisplayObject(0);
    }

}
