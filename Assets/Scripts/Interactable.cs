using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum Interactiontype
    {
        CLICK,
        HOLD,
        LADDER,
        BUTTON,
    }
    //CLICK
    public Interactiontype interactiontype;
    public abstract string GetDescription();
    public abstract void Interact();

    //HOLD
    float HOLDTime;

    public void IncreaseHOLDTime() => HOLDTime += Time.deltaTime;
    public void ResetHOLDTime() => HOLDTime = 0f;

    public float GetHOLDTime() => HOLDTime;

    //LADDER
    public abstract Vector3 OBJECT_ForcePosition();
    public abstract bool UseX();
    public abstract bool UseY();
    public abstract bool UseZ();
}
