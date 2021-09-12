using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public int OxyGenerator_ID = 1;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Changetoactive()
    {
        animator.SetBool("Is_Active", true);
    }
    void Changetoinactive()
    {
        animator.SetBool("Is_Active", false);
    }
}
