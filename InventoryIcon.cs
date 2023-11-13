using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;
using System;

public class InventoryIcon : MonoBehaviour {

    public int amount = 0;
    public int presenting = -1;
    public bool empty = true;
    public List<GameObject> iconSprites; //0-camera, 1-flashlight, 2-wrench, 3-bottle, 4-axe,
                                         //5=Bob'sNote, 6-Sara'sNote, 7-NoteDay1, 8-Note Day 2, 9-Note Day 4, 10-Note Day 6, 11-NoteDead, 12-MapNote
    public TextMeshProUGUI iconDetailsText;
    public string[] iconDetailsList;

    public Image noteImage;
    public Sprite[] noteImageList;

    public TextMeshProUGUI TMPAmount;

    private AudioSource twoDimSource;
    public AudioClip drinkClip, poppinPillsClip, invisChargeClip, injectionClip, hoverClip, clickClip;

    public GameObject NightVisionVol;
    public VisualEffect effect;

    public int startOfNotes;

    private bool move = false;
    private bool up = false;

    public void Start() {
        twoDimSource = GameObject.Find("--- 2D AUDIO SOURCE ---").GetComponent<AudioSource>();
        effect.Stop();
    }

    public void ChangeIcon(int itemCode, int newAmount) {
        empty = false;
        amount = newAmount;
        TMPAmount.text = amount.ToString();
        if(presenting != -1) iconSprites[presenting].SetActive(false);

        presenting = itemCode;
        if(presenting < startOfNotes) iconSprites[itemCode].SetActive(true);
        else iconSprites[startOfNotes].SetActive(true);
    }

    public void Consume() {
        if(presenting == 3) { // aka if this icon is for a soda can
            //DecreaseAmnt();
            GameObject.Find("Player Two").GetComponent<PlayerMovement2>().sprintDuration += 1;
            GameObject.Find("Player Two").GetComponent<Inventory>().RemoveItemFromInventory("SodaCan");
            twoDimSource.PlayOneShot(drinkClip);
        } else if(presenting == 6) { // Syringe
            GameObject.Find("Player Two").GetComponent<PlayerMovement2>().speed += .333333f;
            GameObject.Find("Player Two").GetComponent<Inventory>().RemoveItemFromInventory("Syringe");
            twoDimSource.PlayOneShot(injectionClip);
        }
        else if (presenting == 7) { // Night Pills
            // night vision...how to implement ?
            StartCoroutine(NightVisionTimer());
            GameObject.Find("Player Two").GetComponent<Inventory>().RemoveItemFromInventory("Night Pills");
            twoDimSource.PlayOneShot(poppinPillsClip);
        }
        else if (presenting == 8) { // Invis Battery
            // set enemy to cannot use line of sight to none for 10 sec
            StartCoroutine(InvisibilityTimer());
            GameObject.Find("Player Two").GetComponent<Inventory>().RemoveItemFromInventory("Invisibility Charge");
            twoDimSource.PlayOneShot(invisChargeClip);
        }
    }

    public void IncreaseAmnt() {
        amount++;
        TMPAmount.text = amount.ToString();
    }

    public void DecreaseAmnt() {
        if(amount > 0) {
            amount--;
            TMPAmount.text = amount.ToString();
            if (amount == 0) DumpIcons();
        }
    }

    public void DumpIcons() {
        iconSprites[presenting].SetActive(false);
        empty = true;
        amount = 0;
        TMPAmount.text = "";
        iconDetailsText.text = "";
        presenting = -1;
    }

    public bool ContainsIcon(int itemCode) {
        if (presenting == itemCode) return true;
        else return false;
    }

    public void HoverIcon() {
        twoDimSource.PlayOneShot(hoverClip, .7f);
        if (presenting != -1) {
            iconDetailsText.text = iconDetailsList[presenting];
            if (presenting >= startOfNotes) {
                //noteImage = null;
                //iconDetailsText.color = Color.black;
                noteImage.sprite = noteImageList[presenting - startOfNotes];
                noteImage.enabled = true;
            }
            
        } 
    }

    public void UnhoverIcon() {
        if (presenting != -1) {
            iconDetailsText.text = "";
            //iconDetailsText.color = Color.white;
            if (presenting >= startOfNotes) {
                noteImage.sprite = null;
                noteImage.enabled = false;
                //noteImage = noteImageList[0];
            }
        }
    }

    private void Update() {
        if (move && up) {
            NightVisionVol.GetComponent<Volume>().weight += .1f;
            if (NightVisionVol.GetComponent<Volume>().weight >= 1) move = false;
        } else if (move && !up) {
            NightVisionVol.GetComponent<Volume>().weight -= .1f;
            if (NightVisionVol.GetComponent<Volume>().weight <= 0) {
                move = false;
                NightVisionVol.SetActive(false);
            }
        }
    }

    IEnumerator NightVisionTimer() {
        NightVisionVol.SetActive(true);
        move = true;
        up = true;
        yield return new WaitForSeconds(30f);
        move = true;
        up = false;
    }

    IEnumerator InvisibilityTimer() {//Potential problem, if the crawler is not set to chaseIfSeen originally,
        //this will override that.
        try {
            GameObject.Find("Crawler Body").GetComponent<CrawlerController>().chaseIfSeen = false;
        } catch (Exception e) {
            Debug.Log("No enemy found");
        }
        
        effect.Play();
        yield return new WaitForSeconds(30f);
        effect.Stop();

        try {
            GameObject.Find("Crawler Body").GetComponent<CrawlerController>().chaseIfSeen = true;
        } catch (Exception e) {
            Debug.Log("No enemy found");
        }
    }

}