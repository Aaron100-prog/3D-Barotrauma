using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuctionBoxDoor : Interactable
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override string GetDescription()
    {
        return "Interagieren";
    }

    public override void Interact()
    {
        animator.SetBool("isopening", !animator.GetBool("isopening"));
    }
}
