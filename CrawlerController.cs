using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerController : MonoBehaviour {

    #region General Vars
    public Transform target;
    private GameObject player;
    public bool hunting = false;
    NavMeshAgent agent;
    public Animator crawlAnimator;
    public bool ambushing = false;
    [Header("Below should be empty.")]
    public Transform waitTarget;
    public int ambushChance = 1; // 1=guarunteed, 2=50%, etc.
    #endregion

    #region Audio Vars
    private AudioSource chaseMusic, footstepSource;
    public AudioSource voiceActingSource;
    public AudioClip walkingClip, runningClip, jumpscareClip, breathClip;
    public AudioClip[] VoiceClips;
    public bool canPlayMusic = true;
    #endregion

    #region Chasing Vars
    public bool chaseIfRunning, chaseIfSeen, huntAtStart = false;
    public float chaseSpeed = 33f;
    public float chaseDurationSeconds = 3f;
    #endregion

    #region Private Vars
    private float initSpeed;
    private Coroutine temp;
    private float punish = 0;
    private bool overrideSound = false;
    private int lastLostVAIndex, lastRoarVAIndex = 0;
    #endregion

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        initSpeed = agent.speed;
        footstepSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player Two");
        chaseMusic = GameObject.Find("Chase Music").GetComponent<AudioSource>();

        if (huntAtStart) HuntPlayer();
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
        if (agent.velocity == new Vector3 (0,0,0) || overrideSound) footstepSource.volume = 0;
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
        overrideSound = true;
        yield return new WaitForSeconds(Random.Range(5,45));
        ambushing = false;
        StopHunt(true);
        yield return new WaitForSeconds(4f); //2.65 limit for personal hearing.
        overrideSound = false;
        //footstepSource.volume = .1f;
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
        //footstepSource.volume = 1;
        footstepSource.Play();
    }

    /*
     * HuntPlayer() initiates the audio settings for chasing the player.
     */
    public void HuntPlayer() {
        Debug.Log("Start hunt?");
        PlayVALostPlayer(5, 7, ref lastRoarVAIndex);

        hunting = true;
        agent.speed = chaseSpeed;
        if(canPlayMusic) chaseMusic.Play();
        if (chaseIfSeen) crawlAnimator.Play("Running Crawl");
        footstepSource.clip = runningClip;
        //footstepSource.volume = 1;
        footstepSource.Play();
    }

    /*
     * StopHunt() sets the monster to patrolling values. ie. walking, slowed down, less noisy.
     */
    public void StopHunt(bool delayed) {
        hunting = false;
        agent.speed = initSpeed;
        if (chaseIfSeen) crawlAnimator.Play("Low Crawl");
        chaseMusic.Stop();
        footstepSource.clip = walkingClip;
        //if(!delayed) footstepSource.volume = .1f;
        footstepSource.Play();
    }

    /*
     * BoredTimer() Calls Wait().
     */
    public void BoredTimer() {
        if(temp != null) StopCoroutine(temp);
        temp = StartCoroutine(Wait());
    }

    public void AbandonAndNewTarget(Transform newTarget) {
        AbandonHunt();
        OverrideNewTarget(newTarget);
        crawlAnimator.Play("Running Crawl Leave Area");
        agent.speed = 2;
        crawlAnimator.applyRootMotion = true;
    }

    private void AbandonHunt() {
        chaseIfRunning = false;
        chaseIfSeen = false;
        ambushing = false;
        if (hunting) StopHunt(false);
    }

    private void OverrideNewTarget(Transform newTarget) {
        target = newTarget;
    }

    /*
     * Wait() Is a boredom mechanic for the monster, in which the monster will stop chasing the player if
     * the player has broken line of sight for a determined number of seconds.
     */
    IEnumerator Wait() {
        yield return new WaitForSeconds(chaseDurationSeconds);
        if (!gameObject.GetComponent<LineOfSightChecker>().playerInSight) {
            if (!ambushing) {
                StopHunt(false);

                yield return new WaitForSeconds(Random.Range(0f, 4.4f));

                if (!hunting && !ambushing) {
                    PlayVALostPlayer(0, 5, ref lastLostVAIndex);
                    gameObject.GetComponent<monRanSound>().allowed = false;
                    yield return new WaitForSeconds(5f);
                    gameObject.GetComponent<monRanSound>().allowed = true;
                }
            }
            else {
                chaseMusic.Stop();
            }
        }
    }

    private void PlayVALostPlayer(int minIndex, int maxIndex, ref int tee) {// Change to have a range of clips, a-d is lost player, e-f is seen player, etc.
        int temp = Random.Range(minIndex, maxIndex);
        if (temp == minIndex && temp == tee) temp = minIndex+1;
        else if (temp == tee) temp = minIndex;

        tee = temp;
        voiceActingSource.PlayOneShot(VoiceClips[temp]);
    }

}
