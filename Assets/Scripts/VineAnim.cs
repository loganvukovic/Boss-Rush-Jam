using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineAnim : MonoBehaviour
{


    public Animator animator;
    public bool isanim = false;
    private bool done;


    private void Update()
    {
        if (!done)
        {
            if (isanim)
            {
                Debug.Log("Active");
                animator.SetTrigger("Extend");
                done = true;
            }
        }
    }
}
