using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    public Vector3 Vector = new Vector3(1.0f, 1.0f, 1.0f);
    public override string GetDescription()
    {
        return "Interagieren";
    }

    public override void Interact()
    {
        Debug.Log("Interacted (CLICK)");
    }

    public override Vector3 OBJECT_ForcePosition()
    {
        return Vector;
    }
}
