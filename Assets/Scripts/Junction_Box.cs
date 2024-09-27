using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junction_Box : MonoBehaviour
{
    public float condition;
    public GameObject RepairObject;
    public GameObject[] ConnectedOutputs; 

    public void Repair()
    {
        RepairObject.GetComponent<Renderer>().material.color = new Color();
    }
}
