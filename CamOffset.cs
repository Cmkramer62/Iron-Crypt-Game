using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOffset : MonoBehaviour
{
    private Vector3 vectOffset;
    //public GameObject goFollow;
    public Transform goFollow2;
    //[SerializeField] private float speed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        //goFollow = Camera.main.gameObject;
        //vectOffset = transform.position - goFollow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = goFollow2.transform.position; //+ vectOffset;
        //transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, Time.deltaTime);
    }
}
