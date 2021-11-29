using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    
    public float ID = 1;
    private float neededpower = 15;
    private bool active = true;
    public float throttle = 0;

    public void Update()
    {
        if(active)
        {
            this.transform.parent.parent.transform.position = this.transform.parent.parent.transform.position + (transform.right * throttle/10 * Time.deltaTime);
        }
    }
}
