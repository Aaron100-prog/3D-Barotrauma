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

    private BoxCollider Collider;
    private GameObject WaterParent;
    private GameObject Water;

    public void Start()
    {
        Fillroomwithoxy();
        CreateWater();
    }
    private void CalculateMaxOxygen()
    {
        BoxCollider coll = GetComponent<BoxCollider>();
        MaxOxygenInHull = coll.size.x * coll.size.y * coll.size.z * 15f;
    }

    public void Fillroomwithoxy()
    {
        CalculateMaxOxygen();
        OxygenInHull = MaxOxygenInHull;
    }

    private void CreateWater()
    {
        Collider = GetComponent<BoxCollider>();
        WaterParent = new GameObject("WaterParent " + name);
        WaterParent.transform.SetParent(transform);
        WaterParent.transform.localPosition = new Vector3(0, -(Collider.size.y/2) + Collider.center.y, 0);

        Water = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Water.name = "Water";
        Water.transform.SetParent(WaterParent.transform);
        Water.transform.localPosition = new Vector3(Collider.center.x, Collider.center.y + -WaterParent.transform.localPosition.y, Collider.center.z);
        Water.transform.localScale = Collider.size;
        BoxCollider WaterCollider = Water.GetComponent<BoxCollider>();
        WaterCollider.isTrigger = true;

        WaterParent.transform.localScale = new Vector3(1, 0, 1);
    }
}
