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
    public string[] passedPlayerAccess;


    [HideInInspector]
    public new MeshRenderer renderer;
    Color original;

    void Start()
    {
        renderer = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        original = renderer.material.GetColor("_Color");
    }

    public override string GetDescription()
    {
        return ButtonDescription;
    }

    public override void Interact()
    {
        ActivatedObject.SendMessage("Activate");
        StartCoroutine(AccessgrantedCoroutine());
    }

    IEnumerator AccessgrantedCoroutine()
    {
        renderer.material.SetColor("_Color", Color.green);

        yield return new WaitForSeconds(3);

        renderer.material.SetColor("_Color", new Color(0.8196079f, 0.8196079f, 0.8196079f));
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
