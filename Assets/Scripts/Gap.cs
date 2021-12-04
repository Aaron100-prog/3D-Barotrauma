using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    public GameObject Door;
    [SerializeField]
    public List<Hull> Hulls;
    private LayerMask HullMask;
    public void Start()
    {
        HullMask = LayerMask.GetMask("Hull");
        Collider[] foundhullcollider = Physics.OverlapBox(new Vector3(0, 0, 0), new Vector3(1,1,1), transform.rotation, HullMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < foundhullcollider.Length; i++)
        {
            
            if (foundhullcollider[i].GetComponent<Hull>() == null)
            {
                Debug.Log(gameObject.name + ": " + foundhullcollider[i].gameObject.name + " besitzt keinen Hüllen Script!");
            }
            else
            {
                Hulls.Add(foundhullcollider[i].GetComponent<Hull>());
            }

        }
    }
}
