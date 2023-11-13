using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecurityCode : MonoBehaviour
{

    public bool isRandom = false;
    public string effect = "Door Unlock";
    public string code = "7137";

    public TextMeshProUGUI one, two, three, four;

    public GameObject effectee;
    public AudioSource source;
    public AudioClip clip;

    public TextMeshProUGUI lockStatusText;
    public string doorNameText;

    public bool showObjective;
    public string objectiveText;

    public void codeIncr(int digit) {
        int num = 0;
        if (digit == 1) { num = int.Parse(one.text); }
        else if (digit == 2) { num = int.Parse(two.text); }
        else if (digit == 3) { num = int.Parse(three.text); }
        else if (digit == 4) { num =int.Parse(four.text); }

        if (num == 9) {
            num = 1;
        }
        else {
            num++;
        }

        if (digit == 1) { one.text = num.ToString(); }
        else if (digit == 2) { two.text = num.ToString(); }
        else if (digit == 3) { three.text = num.ToString(); }
        else if (digit == 4) { four.text = num.ToString(); }

        //Debug.Log(four.text);
       // Debug.Log(code);
        string temp = one.text + two.text + three.text + four.text;
        if (temp.Equals(code)) { // CODE CORRECT ?
            
            activateEffect(effect);
        }
    }


    private void activateEffect(string type) {
        if (showObjective) ObjectiveShower.instance.objectiveText.text = objectiveText;//.ShowTextTimed(objectiveText, 10f);

        if(type.Equals("Door Unlock")) {
            lockStatusText.text = doorNameText + ":\nunlocked";
            source.PlayOneShot(clip);
            effectee.GetComponent<DoorScript>().ForceUnlock();
        }



    }
}
