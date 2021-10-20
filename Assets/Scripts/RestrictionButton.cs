using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictionButton : Interactable
{
    [Tooltip("Text der angezeigt wird wenn der Spieler mit dem Cursor �ber dem Knopf ist")]
    public string ButtonDescription;

    [Tooltip("M�ssen alle nicht zugelassen Text Strings �bereinstimmen damit der Knopfdruck nicht erfolgreich ist")]
    public bool CheckallInvalidAccess;
    [Tooltip("Nicht zugelasse Text Strings f�r erfolgreiches dr�cken des Knopfes")]
    public string[] ButtonInvalidAccess;

    [Tooltip("M�ssen alle zugelassen Text Strings �bereinstimmen damit der Knopfdruck erfolgreich ist")]
    public bool CheckallValidAccess;
    [Tooltip("Zugelasse Text Strings f�r erfolgreiches dr�cken des Knopfes")]
    public string[] ButtonValidAccess;

    [Tooltip("Objekt welches nach dr�cken des Knopfes mit ausreichendem Zugriff aktiviert/umgeschalten wird")]
    public GameObject ActivatedObject;

    [HideInInspector]
    public string[] passedPlayerAccess;


    [HideInInspector]
    public new MeshRenderer renderer;
    Color original;

    void Awake()
    {
        interactiontype = Interactiontype.BUTTON;
    }
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
        passedPlayerAccess = null;
    }

    public void GetPlayerAccess(string[] passedAccess)
    {
        passedPlayerAccess = passedAccess;
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
