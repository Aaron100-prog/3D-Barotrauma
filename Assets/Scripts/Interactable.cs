using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum Interactiontype
    {
        CLICK,
    }
    public Interactiontype interactiontype;
    public abstract void Interact();
}
