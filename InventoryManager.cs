using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class InventoryManager : MonoBehaviour {

    public List<GameObject> itemBoxes;
    public Animator inventoryAnim;
    private bool active = false;
    public AudioSource twoDimSource;
    public AudioClip invClip;

    public bool ableToTab = true;

    public void Update() {
        if (ableToTab && Input.GetButtonDown("Tab")) {
            twoDimSource.PlayOneShot(invClip);
            StartCoroutine(animTime());
            if (!active) {
                //GameObject.Find("Main Camera").GetComponent<VignetteAndChromaticAberration>().blur = 1.0f;
                active = true;
                inventoryAnim.Play("InvSlide");
                Cursor.lockState = CursorLockMode.None;
                GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = false;
                ObjectiveShower.ShowText();
            } else {
               // GameObject.Find("Main Camera").GetComponent<VignetteAndChromaticAberration>().blur = 0.0f;
                active = false;
                inventoryAnim.Play("InvSlide2");
                Cursor.lockState = CursorLockMode.Locked;
                GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = true;
                ObjectiveShower.HideText();
            }
        }

    }

    public void AddItem(int itemCode) {
        if(Contains(itemCode) == -1) {
            itemBoxes[FirstEmpty()].GetComponent<InventoryIcon>().ChangeIcon(itemCode, 1);
        } else {
            itemBoxes[Contains(itemCode)].GetComponent<InventoryIcon>().IncreaseAmnt();
        }
    }

    public void RemoveItem(int itemCode) {
        itemBoxes[Contains(itemCode)].GetComponent<InventoryIcon>().DecreaseAmnt();
    }
    
    /* 
     * Returns the index of the first empty box in the icon grid.
     * Returns -1 if the grid is maxed out.
     */
    public int FirstEmpty() {
        int i = 0;
        foreach (GameObject item in itemBoxes) {
            if (item.GetComponent<InventoryIcon>().empty == true) {
                return i;
            }
            i++;
        }
        return -1;
    }

    /*
     * Returns index of icon box in grid, if the icon box contains the item.
     * Returns -1 if Contains() failed to find any icon box containing item.
     */
    public int Contains(int itemCode) {
        int i = 0;
        foreach (GameObject item in itemBoxes) {
            if (item.GetComponent<InventoryIcon>().ContainsIcon(itemCode) == true) {
                return i;
            }
            i++;
        }
        return -1;
    }

    IEnumerator animTime() {
        ableToTab = false;
        yield return new WaitForSeconds(.5f);
        ableToTab = true;
    }
}
