using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    public GameObject Door;
    [SerializeField]
    public List<Hull> Hulls;
    private LayerMask HullMask;
    private GameObject Lowpoint;
    public void Start()
    {
        HullMask = LayerMask.GetMask("Hull");
        Collider[] foundhullcollider = Physics.OverlapBox(transform.localPosition, new Vector3(1,1,1), Quaternion.identity, HullMask, QueryTriggerInteraction.Collide);
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

        Lowpoint = new GameObject("Lowpoint " + name);
        Lowpoint.transform.SetParent(transform);
        Lowpoint.transform.localPosition = new Vector3(0, -0.5f, 0);
    }
}
