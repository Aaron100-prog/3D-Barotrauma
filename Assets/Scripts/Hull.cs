using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : MonoBehaviour
{
    public float MaxOxygenInHull;
    public float OxygenInHull;
    public float WaterLevelInHull = 0f;
    public float PressureInHull = 0f;
    float Volume;
    BoxCollider coll;

    public void Start()
    {
        Fillroomwithoxy();
    }
    private void CalculateMaxOxygen()
    {
        coll = GetComponent<BoxCollider>();
        MaxOxygenInHull = coll.size.x * coll.size.y * coll.size.z * 25f;
    }

    public void Fillroomwithoxy()
    {
        CalculateMaxOxygen();
        OxygenInHull = MaxOxygenInHull;
    }
}
