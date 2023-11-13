using UnityEngine;
using UnityEngine.UI;

public class DoorRaycast : MonoBehaviour {

    public bool viewingAxable = false;

    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;
    [SerializeField] private Image crosshair = null;
    [SerializeField] private Image crosshair2 = null;
    public bool ableToCrosshair = true;
    private bool isCrosshairActive;
    private bool doOnce;
    private HelperText helperTextScript;

    private void Start() {
        helperTextScript = GameObject.Find("Helper Systems").GetComponent<HelperText>();
    }

    void Update() {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask)) {

            if (!doOnce) {
                CrosshairChange(true);
            }
            isCrosshairActive = true;
            doOnce = true;

            if (hit.collider.CompareTag("InteractiveAxable")) {
                viewingAxable = true;
            }

            if (Input.GetKeyDown(openDoorKey)) {

                if (hit.collider.CompareTag("InteractiveObject")) hit.collider.gameObject.GetComponent<DoorScript>().InteractWithDoor();
                else if (hit.collider.CompareTag("InteractiveKey") && !hit.collider.gameObject.GetComponent<KeyScript>().isTaken()) hit.collider.gameObject.GetComponent<KeyScript>().takeKey();
                else if (hit.collider.CompareTag("InteractiveCabinet")) hit.collider.gameObject.GetComponentInParent<NewCabinetUse>().activateCabinet();
                else if (hit.collider.CompareTag("InteractiveSwitch")) hit.collider.gameObject.GetComponent<Activator>().ActivateSwitch();
                else if (hit.collider.CompareTag("InteractiveCrank")) hit.collider.gameObject.GetComponent<Activator>().ActivateSwitch();
                else if (hit.collider.CompareTag("InteractiveScreen")) hit.collider.gameObject.GetComponent<ScreenSet>().enterScreen();
                else if (hit.collider.CompareTag("InteractiveElevSafety")) hit.collider.gameObject.GetComponent<elevatorScript>().openDoors();
                else if (hit.collider.CompareTag("InteractiveElevButton")) hit.collider.gameObject.GetComponent<elevatorScript>().startElevator();
                else if (hit.collider.CompareTag("InteractiveItem")) { //raycast for ITEM
                    hit.collider.gameObject.GetComponent<itemGrabDel>().grabSound.Play();
                    hit.collider.gameObject.GetComponent<itemGrabDel>().move = true;
                    hit.collider.gameObject.GetComponent<itemGrabDel>().ShowObjective();
                    GameObject.Find("Player Two").GetComponent<Inventory>().AddItemToInventory(hit.collider.name);
                }
                else if (hit.collider.CompareTag("InteractiveStatue")) hit.collider.gameObject.GetComponent<dragonAnimate>().turnStatue();
                else if (hit.collider.CompareTag("InteractiveAxable")) HelperText.PopupMessage("I need to use an axe on this...", 4f);
                else if (hit.collider.CompareTag("InteractiveWrenchable")) HelperText.PopupMessage("I need to use a wrench for this...", 4f);
                else if (hit.collider.CompareTag("InteractiveRadio")) hit.collider.gameObject.GetComponent<RadioScript>().Activate();
                else if (hit.collider.CompareTag("InteractiveDrawer")) hit.collider.gameObject.GetComponent<DrawerOpener>().OpenDrawer();
                else if (hit.collider.CompareTag("InteractiveMover")) hit.collider.gameObject.GetComponent<ForceMover>().MoverProcedure();
                
            } else if (Input.GetMouseButtonDown(0)) {
                if (hit.collider.CompareTag("InteractiveAxable")) {
                    if (GameObject.Find("Player Two").GetComponent<Inventory>().heldIndex == 4) {
                        hit.collider.GetComponent<AxeDestruction>().axeDestroyNow();
                    } else {
                        HelperText.PopupMessage("I need to find an axe to break this...",4f);
                    }
                    
                }
                else if (hit.collider.CompareTag("InteractiveWrenchable")){ //raycast for WRENCH        
                    if (GameObject.Find("Player Two").GetComponent<Inventory>().heldIndex == 2) {
                        hit.collider.gameObject.GetComponent<WrenchActivator>().activation();
                    } else {
                        HelperText.PopupMessage("I need to find a wrench to use this...", 4f);
                    }

                }

            }

            if (hit.collider.CompareTag("Crawler")) { // Raycast CRAWLER 
                Debug.Log("Looking at the crawler");
            }

        } else if (Physics.Raycast(transform.position, fwd, out hit, 99, mask) ) {
            if (hit.collider.CompareTag("Crawler")) { // Raycast CRAWLER 
                Debug.Log("Looking at the crawler");
            }
        }


        else {
            if (isCrosshairActive) {
                CrosshairChange(false);
                doOnce = false;
                viewingAxable = false;
            }

        }

    }

    public void CrosshairChange(bool on) {
        if (ableToCrosshair)
        {
            if (on && !doOnce)
            {
                crosshair.color = Color.white;
                crosshair2.color = Color.clear;

            }
            else
            {
                crosshair.color = Color.clear;
                crosshair2.color = Color.white;
                isCrosshairActive = false;
            }
        }
        
    }

}
