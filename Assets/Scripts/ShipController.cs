using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Hull[] hulls;
    private float totalhullsize;
    private float totalneutralvolume;
    private List<Hull> Ballasthulls = new List<Hull>();
    private float totalballastvolume;
    public float neutralballastpercent;

    public float totalwatervolume;
    [SerializeField]
    private float speed = 0;
    public void Start()
    {
        hulls = transform.GetComponentsInChildren<Hull>();
        
        for (int i = 0; i < hulls.Length; i++)
        {
            //Debug.Log(hulls[i].gameObject.name + " " + hulls[i].Isballast);
            totalhullsize += hulls[i].HullVolume;
            if(hulls[i].Isballast)
            {
                Ballasthulls.Add(hulls[i]);
                totalballastvolume += hulls[i].HullVolume;
            }
        }
        totalneutralvolume = totalhullsize / 100 * 10;
        if(totalneutralvolume > totalballastvolume)
        {
            Debug.LogWarning("Das Schiff besitzt zu wenig Ballasthüllen!");
        }
        else
        {
            neutralballastpercent = totalneutralvolume / totalballastvolume * 100;
            for(int i = 0; i < Ballasthulls.Count; i++)
            {
                Ballasthulls[i].forcewaterlevel(neutralballastpercent);
            }
            Pump[] pumps = GetComponentsInChildren<Pump>();
            for (int i = 0; i < pumps.Length; i++)
            {
                if (pumps[i].isballastpump)
                {
                    pumps[i].targetwaterlevel = neutralballastpercent;
                }
            }
        }
        for (int i = 0; i < hulls.Length; i++)
        {
            totalwatervolume += hulls[i].WaterInHull;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float secwatervolume = 0;
        for (int i = 0; i < hulls.Length; i++)
        {
            secwatervolume += hulls[i].WaterInHull;
        }
        totalwatervolume = secwatervolume;
        
        /*if(totalwatervolume > totalneutralvolume)
        {
            speed += 1f;
        }
        if (totalwatervolume < totalneutralvolume)
        {
            speed -= 1f;
        }
        if(System.Math.Round(totalwatervolume, 2) == System.Math.Round(totalneutralvolume, 2))
        {
            Mathf.Lerp(speed, 0f, 2f);
        }

        transform.position = transform.position + (transform.up * speed * Time.deltaTime);
        */
    }
}
