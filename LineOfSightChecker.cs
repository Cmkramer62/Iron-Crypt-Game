using UnityEngine;

public class LineOfSightChecker : MonoBehaviour
{
    private Transform player; // Reference to the player GameObject
    public Transform monster; // Reference to the monster GameObject
    public float fieldOfViewAngle = 90f; // The field of view angle of the monster
    public int viewDistance = 20; // How far away the player can be without being seen.

    public bool playerInSight; // Flag to indicate if the player is in sight

    public LayerMask ignoreMeLayer;

    private CrawlerController enemyScript;
    private bool wasHunting = false;

    private void Start() {
        enemyScript = GameObject.FindGameObjectWithTag("Crawler").GetComponent<CrawlerController>();
        player = GameObject.Find("Player Two").transform;
    }

    /*
     * Update() checks once a frame if the player meets all the requirements to be considered within line of sight.
     */
    private void Update() {
        if (!playerInSight && IsPlayerInViewDistance() && IsPlayerInFieldOfView() && IsPlayerInLineOfSight(player) ) {
            playerInSight = true;
            if(enemyScript.ambushing) enemyScript.StopAmbush();
            if(!enemyScript.hunting && enemyScript.chaseIfSeen == true) enemyScript.HuntPlayer();
        } else if(playerInSight && (!IsPlayerInViewDistance() || !IsPlayerInFieldOfView() || !IsPlayerInLineOfSight(player))){
            enemyScript.BoredTimer();
            playerInSight = false;
        }
    }

    /*
     * IsPlayerInFieldOfView() uses the angle of the Vector3 to the player to see if he is within the field of view angle.
     * Returns true if this is the case.
     */
    private bool IsPlayerInFieldOfView() {
        Vector3 directionToPlayer = player.position - monster.position;
        float angle = Vector3.Angle(monster.forward, directionToPlayer);

        if (angle <= fieldOfViewAngle * 0.5f) {
            return true;
        }

        return false;
    }

    /*
     * IsPlayerInLineOfSight() checks whether the RayCast beam touches something else before it reaches the player.
     * If it touches the player first, then the line of sight is clear, with no obstructions, and returns true.
     */
    private bool IsPlayerInLineOfSight(Transform targetFOV) {
        RaycastHit hit;
        Vector3 bob = new Vector3(targetFOV.position.x, targetFOV.position.y + 1, targetFOV.position.z);
        Vector3 monEyeLevel = new Vector3(monster.position.x, monster.position.y + 3, monster.position.z);
        Vector3 directionToPlayer = bob - monEyeLevel;

        if (Physics.Raycast(monEyeLevel, directionToPlayer, out hit, Mathf.Infinity, ~ignoreMeLayer)) {
            Debug.Log(hit.transform.name);
            if (hit.transform.name.Equals(targetFOV.name)) {
                return true;
            }
        }
        return false;
    }

    private bool IsPlayerInLineOfSight2(Transform targetFOV)
    {
        RaycastHit hit;
        Vector3 bob = new Vector3(targetFOV.position.x, targetFOV.position.y + 1, targetFOV.position.z);
        Vector3 monEyeLevel = new Vector3(monster.position.x, monster.position.y + 3, monster.position.z);
        Vector3 directionToPlayer = bob - monEyeLevel;

        if (Physics.Raycast(monEyeLevel, directionToPlayer, out hit, Mathf.Infinity, ~ignoreMeLayer))
        {
            if (hit.transform.name.Equals(targetFOV.name) || hit.transform.name.Equals("CabinetDoorL") || hit.transform.name.Equals("CabinetDoorR"))
            {
                return true;
            }
        }
        return false;
    }

    /*
     * IsPlayerInViewDistance() checks if the player is within a certain range of the monster.
     * Returns true if this is the case.
     */
    private bool IsPlayerInViewDistance() {
        if(Vector3.Distance(monster.position, player.position) < viewDistance) return true;
        else return false;
    }

    /*
     * CanSeeTarget() is a method that checks what Update() checks every frame. 
     * Returns true if all conditions are met.
     */
    public bool CanSeeTarget(Transform target) {
        return (IsPlayerInViewDistance() && IsPlayerInFieldOfView() && IsPlayerInLineOfSight2(target));
    }
}
