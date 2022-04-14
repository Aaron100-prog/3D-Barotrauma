using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    
    public int ID = 1;
    private float neededpower = 15;
    public int TerminalID = 1;
    private bool active = true;
    public float targetthrottle = 0;
    private float activthrottle;
    public float throttlechange = 2f;

    public void Update()
    {
        if (active)
        {
            if (activthrottle != targetthrottle)
            {
                if (Mathf.RoundToInt(activthrottle) > targetthrottle)
                {
                    float difference = (targetthrottle - activthrottle) * -1;
                    Debug.Log(difference);
                    if (difference > throttlechange)
                    {
                        activthrottle -= throttlechange * Time.deltaTime;
                    }
                    else
                    {
                        activthrottle -= difference * Time.deltaTime;
                        if (System.Math.Round(difference, 2) == 0.00)
                        {
                            activthrottle = targetthrottle;
                        }
                    }
                }
                else
                {
                    float difference = targetthrottle - activthrottle;
                    Debug.Log(difference);
                    if (difference > throttlechange)
                    {
                        activthrottle += throttlechange * Time.deltaTime;
                    }
                    else
                    {
                        activthrottle += difference * Time.deltaTime;
                        if (System.Math.Round(difference, 2) == 0.00)
                        {
                            activthrottle = targetthrottle;
                        }
                    }
                }
            }
            this.transform.parent.parent.transform.position = this.transform.parent.parent.transform.position + (transform.right * activthrottle / 10 * Time.deltaTime);
        }
    }
}
