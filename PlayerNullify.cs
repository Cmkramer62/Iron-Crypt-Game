using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNullify : MonoBehaviour {
    private static bool allow = true;   


    public static void ChangePlayerAbilityPerms() {
        allow = !allow;
        PlayerAbilitiesSet(allow);
    }

    public static void HardSetAllow() {
        PlayerAbilitiesSet(true);
    }

    public static void HardSetDisallow() {
        PlayerAbilitiesSet(false);
    }

    private static void PlayerAbilitiesSet(bool allow) {
        GameObject player = GameObject.Find("Player Two");
        player.GetComponent<PlayerMovement2>().allowedToMove = allow;
        player.GetComponent<Utility>().normUIEnabled = allow;
        player.GetComponent<Utility>().allowedToUseItem = allow;
        player.GetComponent<Inventory>().allowedToCycle = allow;
        player.GetComponent<CharacterController>().enabled = allow;

        GameObject.Find("Helper Systems").GetComponent<InventoryManager>().ableToTab = allow;
        GameObject.Find("Helper Systems").GetComponent<UIController>().canPause = allow;
        GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = allow;
    }

    public static void PlayerAbilitiesSetCameraException(bool allow) {
        GameObject.Find("Main Camera").GetComponent<MouseLook>().allowedToLook = allow;
    }
}
