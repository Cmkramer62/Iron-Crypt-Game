using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTwitch : MonoBehaviour
{

    public Animator animator;

    public bool random = true;
    public string[] twitch = { "EyeTwitch", "EyeTwitch2" };
    public int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomWaitTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RandomWaitTime() {
        yield return new WaitForSeconds(Random.Range(0.0f, 4.0f));
        if (!random) animator.Play(twitch[num]);
        else animator.Play(twitch[Random.Range(0, 2)]);
    }
}
