using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Generator : MonoBehaviour
{
    public GameObject[] ConnectedOutputs;
    public bool activ = false;
    public float savedpower = 0;
    public float maxsavedpower = 1000;
    public float powergeneration = 100;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (activ)
        {
            if (savedpower < maxsavedpower)
            {
                savedpower += powergeneration * Time.deltaTime;
                Debug.Log(savedpower);
            }
        }
    }

    void Activate()
    {
        activ = !activ; 
    }
}
