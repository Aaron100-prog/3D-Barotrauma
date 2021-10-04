using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    public Vector3 Vector = new Vector3(1.0f, 1.0f, 1.0f);
    public bool Use_X = false;
    public bool Use_Y = false;
    public bool Use_Z = false;
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

    public override bool UseX()
    {
        return Use_X;
    }

    public override bool UseY()
    {
        return Use_Y;
    }

    public override bool UseZ()
    {
        return Use_Z;
    }
}
