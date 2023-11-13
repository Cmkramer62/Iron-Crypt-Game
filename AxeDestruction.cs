using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeDestruction : MonoBehaviour
{
    public GameObject dust;
    private Vector3 spawnPosition; // the desired spawn position
    private Quaternion spawnRotation; // the desired spawn rotation


    public void Start()
    {
        spawnPosition = gameObject.transform.position;///new Vector3(0, 0, 0);
        spawnRotation = gameObject.transform.rotation;
    }

    public void axeDestroyNow() {
        StartCoroutine(animTime());
    }

    private IEnumerator animTime() {
        yield return new WaitForSeconds(.6f);
        GameObject.Instantiate(dust, spawnPosition, spawnRotation);
        gameObject.SetActive(false);
    }
}
