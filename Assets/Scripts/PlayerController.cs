﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    [Header("Camera Movement")]
    public float sensitivity = 180f;
    [HideInInspector]
    private float yRotation = 0f;
    public bool Cameracontrol_enabled = true;
    [HideInInspector]
    public Camera Camera;
    private bool Zoomed = false;

    [Header("Player Movement")]
    public bool Playercontrol_enabled = true;
    [HideInInspector]
    public CharacterController Controller;
    [HideInInspector]
    private Vector3 velocity;
    public float walkspeed = 1.5f;
    public float runspeed = 4f;
    public float fallspeed = -10f;
    
    public Transform GroundCheck;
    public LayerMask GroundMask;
    [HideInInspector]
    public float GroundDistance = 0.4f;
    private bool onGround;
    private bool inside;
    public LayerMask HullMask;

    [Header("Swimming")]
    public bool swimming = false;
    public float downdrifting = -0.25f;

    private bool ragdolled = false;

    void Start()
    {
        Camera = this.gameObject.GetComponentInChildren<Camera>();
        Controller = this.gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        inside = Physics.CheckSphere(this.transform.position, 0f, HullMask);
        if(inside && swimming == true)
        {
            EnterHull();
        }
        else if(!inside && swimming == false)
        {
            ExitHull();
        }
        if(swimming == false)
        {
            if (Cameracontrol_enabled == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                yRotation -= mouseY;
                yRotation = Mathf.Clamp(yRotation, -90f, 90f);
                Camera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
                this.transform.Rotate(Vector3.up * mouseX);

                if(Input.GetMouseButtonDown(1))
                {
                    Zoomed = true;
                }
                if(Input.GetMouseButtonUp(1))
                {
                    Zoomed = false;
                }
                if (Zoomed)
                {
                    Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 20, Time.deltaTime * 2);
                }
                else
                {
                    Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 60, Time.deltaTime * 2);
                }
            }
            if (Playercontrol_enabled)
            {
                onGround = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                Vector3 move = transform.right * x + transform.forward * z;
                if (Input.GetKey("left shift"))
                {
                    Controller.Move(move * runspeed * Time.deltaTime);
                }
                else
                {
                    Controller.Move(move * walkspeed * Time.deltaTime);
                }

                velocity.y += fallspeed * Time.deltaTime;
                Controller.Move(velocity * Time.deltaTime);
            }
        }
        else
        {
            if (Cameracontrol_enabled == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                this.transform.Rotate(Vector3.left * mouseY + Vector3.up * mouseX);

                if (Input.GetMouseButtonDown(2))
                {
                    Zoomed = true;
                }
                if (Input.GetMouseButtonUp(2))
                {
                    Zoomed = false;
                }
                if (Zoomed)
                {
                    Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 20, Time.deltaTime * 2);
                }
                else
                {
                    Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 60, Time.deltaTime * 2);
                }
            }
            if (Playercontrol_enabled)
            {
                if (Input.GetKey("x"))
                {
                    Vector3 direction = new Vector3(0, transform.rotation.eulerAngles.y, 0);
                    Quaternion targetRotation = Quaternion.Euler(direction);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 2);
                }

                float z = Input.GetAxis("Vertical");
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Jump");

                float rollspeed = walkspeed * 50f;
                float rollz = Input.GetAxis("Roll") * rollspeed * Time.deltaTime;
                this.transform.Rotate(Vector3.back * rollz);

                Vector3 move = transform.up * y + transform.right * x + transform.forward * z;
                float swimspeed = walkspeed * 0.4f;
                float fastswimspeed = runspeed * 0.4f;

                if (Input.GetKey("left shift"))
                {
                    Controller.Move(move * fastswimspeed * Time.deltaTime);
                }
                else
                {
                    Controller.Move(move * swimspeed * Time.deltaTime);
                }
                
                if(ragdolled == true)
                {
                    velocity.y = downdrifting;
                    Controller.Move(velocity * Time.deltaTime);
                }
                
            }
        }
        if(Input.GetKeyDown("c"))
        {
            EnterHull();
        }
        if (Input.GetKeyDown("v"))
        {
            ExitHull();
        }
    }

    public void EnterHull()
    {
        swimming = false;

        Camera.transform.Rotate(Vector3.up * this.transform.localRotation.x);

        StartCoroutine("ResetRotation");
    }

    public void ExitHull()
    {
        swimming = true;

        this.transform.localRotation = Quaternion.Euler(Camera.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        Camera.transform.localRotation = Quaternion.Euler(0, 0, 0);

        StopCoroutine("ResetRotation");
    }

    IEnumerator ResetRotation()
    {
        while (System.Math.Round(this.transform.eulerAngles.z,2) != 0 && System.Math.Round(this.transform.eulerAngles.x,2) != 0)
        {
            Vector3 direction = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            Quaternion targetRotation = Quaternion.Euler(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 2);
            yield return null;
        }
        this.transform.rotation = Quaternion.Euler( 0, transform.eulerAngles.y, 0);
    }
}
