using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum Interactiontype
    {
        CLICK,
        HOLD,
    }
    public Interactiontype interactiontype;
    public abstract string GetDescription();
    public abstract void Interact();

    float HOLDTime;

    public void IncreaseHOLDTime() => HOLDTime += Time.deltaTime;
    public void ResetHOLDTime() => HOLDTime = 0f;

    public float GetHOLDTime() => HOLDTime;
}
