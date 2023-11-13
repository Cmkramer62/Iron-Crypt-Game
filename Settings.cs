using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void backToMain() {
        mainMenu.SetActive(true);
        mainMenu.GetComponent<ButtonListSlide>().Start();
        settingsMenu.SetActive(false);
    }

}
