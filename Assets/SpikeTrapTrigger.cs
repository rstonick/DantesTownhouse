using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapTrigger : MonoBehaviour
{
    private Animator animator;

    private void Update()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider c)
    {

        if (c.attachedRigidbody is not null)
        {
            
            SpikeTrapPerson stp = c.GetComponent<SpikeTrapPerson>();
            if (stp is not null) 
            {
                Debug.Log("enter");
                Debug.Log(stp.name);
                animator.SetBool("idle", false);
                animator.SetBool("down", false);
                animator.SetBool("up", true);
                stp.HitTrap();
            }
        }

    }
 
    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody is not null)
        {
            
            SpikeTrapPerson stp = c.GetComponent<SpikeTrapPerson>();
            if (stp is not null)
            {
                Debug.Log("exit");
                Debug.Log(stp.name);
                animator.SetBool("idle", false);
                animator.SetBool("up", false);
                animator.SetBool("down", true);
            }
            
        }
    }
}
