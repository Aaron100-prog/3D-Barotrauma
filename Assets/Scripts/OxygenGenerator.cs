using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenGenerator : MonoBehaviour
{
    public float power = 100f;

    public Fan[] Fans;

    public void Start()
    {
        Fans = transform.parent.GetComponentsInChildren<Fan>();
    }

    public void Update()
    {
        if (power > 0f)
        {
            for(int i = 0; i < Fans.Length; i++)
            {
                if(!Fans[i].active)
                {
                    Fans[i].Changetoactive();
                }
            }
        }
        else
        {
            for (int i = 0; i < Fans.Length; i++)
            {
                if (Fans[i].active)
                {
                    Fans[i].Changetoinactive();
                }
            }
        }
    }
}
