using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item {
    public string name = "";
    public int amount;
    public string type = "";//consumable, note, important

    public bool throwable;

    public Item (string name, int amount, string type, bool throwable) {
        this.name = name;
        this.amount = amount;
        this.type = type;
        this.throwable = throwable;
    }
}

public class Inventory : MonoBehaviour {
    public bool allowedToCycle = true;

    public Animator swapperAnimator;
    public GameObject[] playerItemMeshes;
    public int heldIndex = 0;
    List<Item> heldList = new List<Item>();
    public InventoryManager managerScript;
    public AudioSource twoDimSource;
    public AudioClip swapClip;

    // Start is called before the first frame update
    void Start() { // These items should be all the possible items that the player could get, right from the very beginning.
        Item camera = new Item ("Camera", 1, "Important", false);
        heldList.Add(camera);
        managerScript.AddItem(0);
        Item box = new Item("Flashlight", 1, "Important", true);
        heldList.Add(box);
        managerScript.AddItem(1);
        Item wrench = new Item("Wrench", 0, "Important", true);
        heldList.Add(wrench);
        Item bottle = new Item("SodaCan", 0, "Consumable", true);
        heldList.Add(bottle);
        Item axe = new Item("Axe", 0, "Important", true);
        heldList.Add(axe);
        Item key = new Item("Storage Room Key", 0, "Consumable", false);
        heldList.Add(key);
        Item syringe = new Item("Syringe", 0, "Consumable", false);
        heldList.Add(syringe);
        Item nightpills = new Item("Night Pills", 0, "Consumable", false);
        heldList.Add(nightpills);
        Item invischarge = new Item("Invisibility Charge", 0, "Consumable", false);
        heldList.Add(invischarge);

    }

    // Update is called once per frame
    void Update() {
        if (allowedToCycle && Input.GetAxis("Mouse ScrollWheel") > 0f) {
            CycleUp();
        }
        else if (allowedToCycle && Input.GetAxis("Mouse ScrollWheel") < 0f) {
            CycleDown();
        }

    }

    //If the item is in the list of things the player could possibly get.
    public bool PotentiallyInInventory(string name) {
        foreach (Item item in heldList) {
            if (item.name.Equals(name)) {
                return true;
            }
        }
        return false;
    }

    //If the item is actually in the inventory.
    public bool InInventory(string name) {
        foreach (Item item in heldList) {
            if (item.name.Equals(name) && item.amount > 0) {
                Debug.Log("amount " + item.amount);
                return true;
            }
        }
        return false;
    }

    public int ItemsIndex(string name) {
        if (PotentiallyInInventory(name)) {
            int i = 0;
            foreach (Item item in heldList) {
                if (item.name.Equals(name)) {
                    return i;
                }
                i++;
            }
        } 
        return -1;  
    }

    public void AddItemToInventory(string name) {
        for(int i = 0; i < heldList.Count; i++) {
            if (heldList[i].name.Equals(name)) {
                heldList[i].amount++;
                managerScript.AddItem(i);
            }
        }
    }

    public void RemoveItemFromInventory(string name) {
        for(int i = 0; i < heldList.Count; i++) {
            if (heldList[i].name.Equals(name) && heldList[i].amount > 0) {
                heldList[i].amount--;
                managerScript.RemoveItem(i);
            }
        }
    }

    #region ------------------------ SWAPPING HELD ITEMS
    public void CycleUp() {
        twoDimSource.PlayOneShot(swapClip);
        bool found = false;
        for (int i = heldIndex + 1; i < heldList.Count; i++) {
            if (heldList[i].amount > 0 && heldList[i].type.Equals("Important")) {
                found = true;
                swapperAnimator.Play("SwapAnim");
                StartCoroutine(AnimationTimer(playerItemMeshes[heldIndex], playerItemMeshes[i]));
                heldIndex = i;
                break;
            }
        }
        if (!found) {
            for (int i = 0; i < heldIndex; i++) {
                if (heldList[i].amount > 0 && heldList[i].type.Equals("Important")) {
                    swapperAnimator.Play("SwapAnim");
                    StartCoroutine(AnimationTimer(playerItemMeshes[heldIndex], playerItemMeshes[i]));
                    heldIndex = i;
                    break;
                }
            }
        }
    }

    public void CycleDown() {
        twoDimSource.PlayOneShot(swapClip);
        bool found = false;
        if (heldIndex != 0) {
            for (int i = heldIndex - 1; i >= 0; i--) {
                if (heldList[i].amount > 0 && heldList[i].type.Equals("Important")) {
                    found = true;
                    swapperAnimator.Play("SwapAnim");
                    StartCoroutine(AnimationTimer(playerItemMeshes[heldIndex], playerItemMeshes[i]));
                    heldIndex = i;
                    break;
                }
            }
        }
        if (!found) {
            for (int i = heldList.Count - 1; i > heldIndex; i--) {
                if (heldList[i].amount > 0 && heldList[i].type.Equals("Important")) {
                    swapperAnimator.Play("SwapAnim");
                    StartCoroutine(AnimationTimer(playerItemMeshes[heldIndex], playerItemMeshes[i]));
                    heldIndex = i;
                    break;
                }
            }
        }
    }

    public IEnumerator AnimationTimer(GameObject swapFrom, GameObject swapTo) {
        yield return new WaitForSeconds(.35f);
        swapFrom.SetActive(false);
        swapTo.SetActive(true);
    }
    #endregion
}
