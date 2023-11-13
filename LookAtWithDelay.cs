using UnityEngine;

public class LookAtWithDelay : MonoBehaviour {
    public bool working = true;
    public bool defaultToPlayer = true;
    public Transform targetObject; // The object to look at
    public float rotationSpeed = 1.0f; // The speed of rotation
    public int maxDistance = 30;

    public AudioSource cameraSound;

    private void Start() {
        if (defaultToPlayer) targetObject = GameObject.Find("Main Camera").transform;
    }

    private void Update() {
        if (working && Vector3.Distance(transform.position, targetObject.position) < maxDistance) {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(targetObject.position - transform.position);
            if(Quaternion.Angle(transform.rotation, targetRotation) > 1 && !cameraSound.isPlaying) {
                cameraSound.Play();
            } else if(cameraSound.isPlaying && Quaternion.Angle(transform.rotation, targetRotation) < 1) {
                cameraSound.Pause();
            }

            // Apply rotation gradually using Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
