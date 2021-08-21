using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickInteractable : Interactable
{
    public override void Interact()
    {
        Debug.Log("Interacted (CLICK)");
    }

    public override string GetDescription()
    {
        return "Interagieren";
    }

    public override Vector3 OBJECT_ForcePosition()
    {
        throw new System.NotImplementedException();
    }

}
