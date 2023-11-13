using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public bool isInRange = true;
    public KeyCode interactKey;
    private Animator animator;
    public bool isOpen = true;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                //animator.SetBool("toClose", true);
                //animator.SetBool("toOpen", true);

                
                if (isOpen){
                    Debug.Log("close it");
                    animator.SetBool("toClose", true);
                    animator.SetBool("toOpen", false);
                    isOpen = false;
                    //closeDoor();
                }
                else{
                    Debug.Log("open it");
                    animator.SetBool("toOpen", true);
                    animator.SetBool("toClose", false);
                    isOpen = true;
                    //openDoor();
                }
                
            }
        }
    }
    /*
    public void openDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetBool("toOpen", isOpen);

        }
        animator.SetBool("toClose", false);
        animator.SetBool("toOpen", false);
    }

    
    public void closeDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            //animator.SetBool("toClose", true);
        }
        animator.SetBool("toClose", false);
        animator.SetBool("toOpen", false);
    }
    */
}
