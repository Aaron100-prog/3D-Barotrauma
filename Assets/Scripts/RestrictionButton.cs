using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictionButton : Interactable
{
    [Tooltip("Text der angezeigt wird wenn der Spieler mit dem Cursor über dem Knopf ist")]
    public string ButtonDescription;

    [Tooltip("Müssen alle nicht zugelassen Text Strings übereinstimmen damit der Knopfdruck nicht erfolgreich ist")]
    public bool CheckallInvalidAccess;
    [Tooltip("Nicht zugelasse Text Strings für erfolgreiches drücken des Knopfes")]
    public string[] ButtonInvalidAccess;

    [Tooltip("Müssen alle zugelassen Text Strings übereinstimmen damit der Knopfdruck erfolgreich ist")]
    public bool CheckallValidAccess;
    [Tooltip("Zugelasse Text Strings für erfolgreiches drücken des Knopfes")]
    public string[] ButtonValidAccess;

    [Tooltip("Objekt welches nach drücken des Knopfes mit ausreichendem Zugriff aktiviert/umgeschalten wird")]
    public GameObject ActivatedObject;

    [HideInInspector]
    public string[] passedPlayerInvalidAccess;
    [HideInInspector]
    public string[] passedPlayerValidAccess;

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
