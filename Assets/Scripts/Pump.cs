using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    public float ID = 1;
    private float neededpower = 10;
    private bool active = true;
    public float targetwaterlevel = 0;
    public float waterchange = 20f;
    private LayerMask HullMask;
    private Hull thishull;
    [HideInInspector]
    public bool isballastpump;

    public void Start()
    {
        HullMask = LayerMask.GetMask("Hull");
        Collider[] hitcollider = Physics.OverlapSphere(this.transform.position, 0f, HullMask, QueryTriggerInteraction.Collide);
        if (hitcollider.Length != 0)
        {
            Hull hull = hitcollider[0].gameObject.GetComponent<Hull>();
            if (hull != null)
            {
                thishull = hull;
                if(thishull.Isballast)
                {
                    isballastpump = true;
                }
            }
            else
            {
                Debug.Log(gameObject.name + ": " + hitcollider[0].gameObject.name + " besitzt keinen Hüllen Script!");
            }
        }
    }

    public void Update()
    {
        if (active)
        {
            if (Mathf.RoundToInt(thishull.WaterInHull / thishull.HullVolume * 100) != targetwaterlevel)
            {
                if(Mathf.RoundToInt(thishull.WaterInHull / thishull.HullVolume * 100) > targetwaterlevel)
                {
                    float difference = ((targetwaterlevel / 100 * thishull.HullVolume) - thishull.WaterInHull) * -1;
                    Debug.Log(difference);
                    if (difference > waterchange)
                    {
                        thishull.WaterInHull -= waterchange * Time.deltaTime;
                    }
                    else
                    {
                        thishull.WaterInHull -= difference * Time.deltaTime;
                        if (System.Math.Round(difference, 2) == 0.00)
                        {
                            thishull.WaterInHull = targetwaterlevel / 100 * thishull.HullVolume;
                        }
                    }
                }
                else
                {
                    float difference = ((targetwaterlevel / 100 * thishull.HullVolume) - thishull.WaterInHull);
                    Debug.Log(difference);
                    if (difference > waterchange)
                    {
                        thishull.WaterInHull += waterchange * Time.deltaTime;
                    }
                    else
                    {
                        thishull.WaterInHull += difference * Time.deltaTime;
                        if (System.Math.Round(difference, 2) == 0.00)
                        {
                            thishull.WaterInHull = targetwaterlevel / 100 * thishull.HullVolume;
                        }
                    }
                }
            }
        }
        
    }
}
