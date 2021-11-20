using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : MonoBehaviour
{
    private float MaxOxygenInHull = 1000f;
    public float OxygenInHull = 1000f;
    public float WaterLevelInHull = 0f;
    public float PressureInHull = 0f;

    public void Start()
    {
        Fillroomwithoxy();
    }

    public void Fillroomwithoxy()
    {
        OxygenInHull = MaxOxygenInHull;
    }
}
