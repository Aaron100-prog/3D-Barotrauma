using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    public GameObject Door;
    [SerializeField]
    Collider[] foundhulls;

    public void Start()
    {
        foundhulls = Physics.OverlapBox(this.transform.position, this.transform.localScale, this.transform.rotation, 9, QueryTriggerInteraction.Ignore);
        
    }
}
