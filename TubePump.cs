using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubePump : MonoBehaviour {

    public GameObject[] Pipes;
    public Material pipeStagnant, pipeMoving;

    public Animator MotorArmsAnimator;
    public MeshRenderer lightBaseMesh, lightBulbMesh;
    public Material lightBaseMat, lightBulbMat, lightBaseDeadMat, lightBulbDeadMat;

    // Start is called before the first frame update
    void Start() {
        foreach(GameObject pipe in Pipes) {
            pipe.gameObject.GetComponent<MeshRenderer>().material = pipeStagnant;
        }

        lightBaseMesh.material = lightBaseDeadMat;
        lightBulbMesh.material = lightBulbDeadMat;
    }

    public void ActivatePump() {
        foreach (GameObject pipe in Pipes) {
            pipe.gameObject.GetComponent<MeshRenderer>().material = pipeMoving;
        }

        lightBaseMesh.material = lightBaseMat;
        lightBulbMesh.material = lightBulbMat;

        MotorArmsAnimator.Play("MotorArmsAnim");
    }



}
