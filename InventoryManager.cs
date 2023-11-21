using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class InventoryManager : MonoBehaviour {

    public List<GameObject> itemBoxes;
    public Animator tabAnimator, rotateTabAnimator;
    private bool active = false;
    public AudioSource twoDimSource, twoDimQuietSource;
    public AudioClip invClip, rotateClip;

    public bool ableToTab = true;

    private bool rotateCooldown = false;

    public CanvasGroup[] labelList;
    private int hightlightLabel = 0;

    public void Update() {
        if (ableToTab && Input.GetButtonDown("Tab")) {
            TabSlide();
        }
    }

    public void TabSlide() {
        twoDimSource.PlayOneShot(invClip);
        StartCoroutine(animTime());
        if (!active) {
            //GameObject.Find("Main Camera").GetComponent<VignetteAndChromaticAberration>().blur = 1.0f;
            active = true;
            tabAnimator.Play("InvSlide");
            Cursor.lockState = CursorLockMode.None;
            GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = false;
            ObjectiveShower.ShowText();
        } else {
            // GameObject.Find("Main Camera").GetComponent<VignetteAndChromaticAberration>().blur = 0.0f;
            active = false;
            tabAnimator.Play("InvSlide2");
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = true;
            ObjectiveShower.HideText();
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

    public void RotateZeroOne(bool reverse) {
        if (!rotateCooldown) {
            if (!reverse) {
                rotateTabAnimator.Play("Tab 0-1");
                SwitchLabel(0,1);
            }
            else {
                rotateTabAnimator.Play("Tab 1-0");
                SwitchLabel(1,0);
            }
            StartCoroutine(RotateCooldownTimer(true));
        }
    }

    public void RotateOneTwo(bool reverse) {
        if (!rotateCooldown) {
            if (!reverse) {
                rotateTabAnimator.Play("Tab 1-2");
                SwitchLabel(1,2);
            }
            else {
                rotateTabAnimator.Play("Tab 2-1");
                SwitchLabel(2,1);
            }
            StartCoroutine(RotateCooldownTimer(true));
        }
    }

    public void RotateTwoThree(bool reverse) {
        if (!rotateCooldown) {
            if (!reverse) {
                rotateTabAnimator.Play("Tab 2-3");
                SwitchLabel(2,3);
            }
            else {
                rotateTabAnimator.Play("Tab 3-2");
                SwitchLabel(3,2);
            }
            StartCoroutine(RotateCooldownTimer(true));
        }
    }

    public void RotateThreeZero(bool reverse) {
        if (!rotateCooldown) {
            if (!reverse) {
                rotateTabAnimator.Play("Tab 3-0");
                SwitchLabel(3,0);
            }
            else {
                rotateTabAnimator.Play("Tab 0-3");
                SwitchLabel(0,3);
            }
            StartCoroutine(RotateCooldownTimer(true));
        }
    }

    private IEnumerator RotateCooldownTimer(bool SFX) {
        if (SFX) twoDimQuietSource.PlayOneShot(rotateClip, .7f);
        rotateCooldown = true;
        yield return new WaitForSeconds(.5f);
        rotateCooldown = false;
    }

    public void HighlightLabel(int index) {
        hightlightLabel = index;
    }

    private void SwitchLabel(int indexStart, int indexEnd) {
        labelList[indexStart].alpha = .2f;
        labelList[indexEnd].alpha = 1f;
    }


}
