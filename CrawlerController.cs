using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerController : MonoBehaviour {

    #region General Vars
    public Transform target;
    public GameObject player;
    public bool hunting = false;
    NavMeshAgent agent;
    public Animator crawlAnimator;
    public bool ambushing = false;
    public Transform waitTarget;
    #endregion

    #region Audio Vars
    public AudioSource chaseMusic, footstepSource;
    public AudioClip walkingClip, runningClip, jumpscareClip;
    public bool canPlayMusic = true;
    #endregion

    #region Chasing Vars
    public bool chaseIfRunning, chaseIfSeen = false;
    public float chaseSpeed = 33f;
    public float chaseDurationSeconds = 3f;
    #endregion

    #region Private Vars
    private float initSpeed;
    private Coroutine temp;
    private float punish = 0;
    #endregion

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        initSpeed = agent.speed;
        footstepSource = gameObject.GetComponent<AudioSource>();
    }

    void Update() {
        #region TARGET DESTINATION
        if (hunting && !ambushing) {
            agent.SetDestination(player.transform.position);
        } else if (ambushing) {
            agent.SetDestination(waitTarget.position);
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            lookDirection.y = 0f;
            if (lookDirection != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        } else if (!hunting && !ambushing) {
            agent.SetDestination(target.position);
        }
        #endregion

        #region VOLUME CONTROL
        if (agent.velocity == new Vector3 (0,0,0)) footstepSource.volume = 0;
        else if(hunting && !ambushing) footstepSource.volume = 1;
        else if(!hunting && !ambushing) footstepSource.volume = .1f;
        #endregion

        #region AUDIO BASED HUNTING
        if (player.GetComponent<PlayerMovement2>().isSprinting && punish < 1001) {
            punish++;
        } else if (punish > 0) {
            punish -= .1f;
        }
        if (chaseIfRunning && punish > 1000 && !hunting) {
            HuntPlayer();
        }
        #endregion
    }

    /*
     * RandomAmbush() starts a Coroutine to have the monster amush the player based on random chance of 1/int chance.
     */
    public void RandomAmbush(int chance) {
        int ran = Random.Range(0, chance);
        if (ran == 0) {
            StartCoroutine(AmbushPlayer());
        }
    }

    /*
     * AmbushPlayer() Has the monster wait in front of a locker, for a preset amount of time.
     */
    public IEnumerator AmbushPlayer() {
        ambushing = true;
        gameObject.GetComponent<monRanSound>().allowed = false;
        crawlAnimator.Play("Sniff");
        footstepSource.volume = 0f;

        //wait for time between 5-13 seconds.
        yield return new WaitForSeconds(Random.Range(5,45));
        ambushing = false;
        StopHunt();
    }

    public void StopAmbush() {
        StartCoroutine(AmbushScare());
    }

    /*
     * AmbushScare() initiates the audio settings for chasing the player.
     */
    private IEnumerator AmbushScare() {
        chaseMusic.Play();
        chaseMusic.PlayOneShot(jumpscareClip, 0.7f);
        yield return new WaitForSeconds(.5f);
        ambushing = false;
        crawlAnimator.Play("Running Crawl");
        footstepSource.clip = runningClip;
        footstepSource.volume = 1;
        footstepSource.Play();
    }

    /*
     * HuntPlayer() initiates the audio settings for chasing the player.
     */
    public void HuntPlayer() {
        hunting = true;
        agent.speed = chaseSpeed;
        if(canPlayMusic) chaseMusic.Play();
        if (chaseIfSeen) crawlAnimator.Play("Running Crawl");
        footstepSource.clip = runningClip;
        footstepSource.volume = 1;
        footstepSource.Play();
    }

    /*
     * StopHunt() sets the monster to patrolling values. ie. walking, slowed down, less noisy.
     */
    public void StopHunt() {
        hunting = false;
        agent.speed = initSpeed;
        if (chaseIfSeen) crawlAnimator.Play("Low Crawl");
        chaseMusic.Stop();
        footstepSource.clip = walkingClip;
        footstepSource.volume = .1f;
        footstepSource.Play();
    }

    /*
     * BoredTimer() Calls Wait().
     */
    public void BoredTimer() {
        if(temp != null) StopCoroutine(temp);
        temp = StartCoroutine(Wait());
    }

    /*
     * Wait() Is a boredom mechanic for the monster, in which the monster will stop chasing the player if
     * the player has broken line of sight for a determined number of seconds.
     */
    IEnumerator Wait() {
        yield return new WaitForSeconds(chaseDurationSeconds);
        if (!gameObject.GetComponent<LineOfSightChecker>().playerInSight) {
            if (!ambushing) StopHunt();
            else {
                chaseMusic.Stop();
            }
        }
    }

}
