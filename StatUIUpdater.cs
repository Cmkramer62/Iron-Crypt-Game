using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatUIUpdater : MonoBehaviour {

    public Slider statSlider;
    public TextMeshProUGUI TMPtext;

    private GameObject PT;

    private void Start() {
        PT = GameObject.Find("Player Two");

        if (gameObject.CompareTag("SprintDuration")) UpdateStat(PT.GetComponent<PlayerMovement2>().sprintDuration);
        else if (gameObject.CompareTag("SprintSpeed")) UpdateStat(PT.GetComponent<PlayerMovement2>().speed);
        else if (gameObject.CompareTag("HealthMeter")) UpdateHealth(3);
    }

    public void UpdateStat(float newValue) {
        statSlider.value = ((short)newValue);
        
        string append = "";

        if (gameObject.CompareTag("SprintSpeed")) append = " meters/sec";
        else if (gameObject.CompareTag("SprintDuration")) append = " seconds";
        TMPtext.text = newValue.ToString() + append;
    }

    public void UpdateHealth(int health) {
        if (health == 1) { 
            TMPtext.text = "You feel like you are about to pass out.";
            gameObject.GetComponentInChildren<Animator>().Play("Fastest");
            gameObject.GetComponentInChildren<Image>().color = Color.red;
        }
        if (health == 2) { 
            TMPtext.text = "You don't feel too good.";
            gameObject.GetComponentInChildren<Animator>().Play("Fast");
            gameObject.GetComponentInChildren<Image>().color = Color.yellow;
        }
        if (health == 3) { 
            TMPtext.text = "You feel alright.";
            Debug.Log(gameObject.name);
            gameObject.GetComponentInChildren<Animator>().Play("Normal");
            gameObject.GetComponentInChildren<Image>().color = Color.green;
        }
    }

}
