using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenGenerator : MonoBehaviour
{
    public int ID;
    public float power = 100f;

    public List<Fan> FanswithID;

    public void Start()
    {
        Fan[] AllFans = transform.parent.GetComponentsInChildren<Fan>();
        for (int i = 0; i < AllFans.Length; i++)
        {
            if (AllFans[i].OxyGenerator_ID == ID)
            {
                FanswithID.Add(AllFans[i]);
            }
        }
    }

    public void Update()
    {
        if (power > 0f)
        {
            for(int i = 0; i < FanswithID.Count; i++)
            {
                if(!FanswithID[i].active)
                {
                    FanswithID[i].Changetoactive();
                }
            }
        }
        else
        {
            for (int i = 0; i < FanswithID.Count; i++)
            {
                if (FanswithID[i].active)
                {
                    FanswithID[i].Changetoinactive();
                }
            }
        }
    }
}
