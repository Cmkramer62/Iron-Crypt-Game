using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneAccess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Don't rename! Unity doesn't handle renaming for OnClick() Events.
    public void loadScene(int x) {
        SceneManager.LoadScene(x);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
