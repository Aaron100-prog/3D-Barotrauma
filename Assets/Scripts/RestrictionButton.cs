using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictionButton : Interactable
{
    public string ButtonDescription;

    public bool CheckallInvalidAccess;
    public string[] ButtonInvalidAccess;

    public bool CheckallValidAccess;
    public string[] ButtonValidAccess;

    public GameObject ActivatedObject;

    [HideInInspector]
    public string[] PlayerInvalidAccess;
    [HideInInspector]
    public string[] PlayerValidAccess;

    public override string GetDescription()
    {
        return ButtonDescription;
    }

    public override void Interact()
    {
        ActivatedObject.SendMessage("Activate");
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
