using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuctionBoxDoor : Interactable
{
    Animator animator;
    void Awake()
    {
        interactiontype = Interactiontype.HOLD;
    }
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

    public override Vector3 OBJECT_ForcePosition()
    {
        throw new System.NotImplementedException();
    }

    public override bool UseX()
    {
        throw new System.NotImplementedException();
    }

    public override bool UseY()
    {
        throw new System.NotImplementedException();
    }

    public override bool UseZ()
    {
        throw new System.NotImplementedException();
    }
}
