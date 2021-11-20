using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public int OxyGenerator_ID = 1;

    public bool active = true;
    Animator animator;
    Hull thishull;
    public LayerMask HullMask;

    void Start()
    {
        animator = GetComponent<Animator>();

        Collider[] hitcollider = Physics.OverlapSphere(this.transform.position, 0f, HullMask, QueryTriggerInteraction.Collide);
        if (hitcollider.Length != 0)
        {
            Hull hull = hitcollider[0].gameObject.GetComponent<Hull>();
            if (hull != null)
            {
                thishull = hull;
            }
            else
            {
                Debug.Log(gameObject.name + ": " + hitcollider[0].gameObject.name + " besitzt keinen Hüllen Script!");
            }

        }
    }
    private void Update()
    {
        if(active)
        {
            if(thishull.OxygenInHull < thishull.MaxOxygenInHull - 5f)
            {
                thishull.OxygenInHull = thishull.OxygenInHull + 5f * Time.deltaTime;
            }
            else
            {
                float difference = thishull.MaxOxygenInHull - thishull.OxygenInHull;
                thishull.OxygenInHull = thishull.OxygenInHull + difference;
            }
        }
    }

    void Changetoactive()
    {
        animator.SetBool("Is_Active", true);
        active = true;
    }
    void Changetoinactive()
    {
        animator.SetBool("Is_Active", false);
        active = false;
    }
}
