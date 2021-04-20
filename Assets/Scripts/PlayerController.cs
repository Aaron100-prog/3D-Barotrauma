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

    [Header("Player Movement")]
    public bool Playercontrol_enabled = true;
    [HideInInspector]
    public CharacterController Controller;
    [HideInInspector]
    private Vector3 velocity;
    public float walkspeed = 6f;
    public float fallspeed = -10f;
    
    public Transform GroundCheck;
    public LayerMask GroundMask;
    public float GroundDistance = 0.4f;
    private bool onGround;

    void Start()
    {
        Camera = this.gameObject.GetComponentInChildren<Camera>();
        Controller = this.gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Cameracontrol_enabled == true)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);
            Camera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            this.transform.Rotate(Vector3.up * mouseX);
        }
        if (Playercontrol_enabled)
        {
            onGround = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            Controller.Move(move * walkspeed * Time.deltaTime);
            velocity.y += fallspeed * Time.deltaTime;
            Controller.Move(velocity * Time.deltaTime);
        }
    }
}
