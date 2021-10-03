using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreecamController : MonoBehaviour
{
    [Header("Camera Movement")]
    private float sensitivity = 210f;
    private float flyingspeed = 2f;
    private float fastflyingspeed = 7f;
    [HideInInspector]
    private float yRotation = 0f;
    private float xRotation = 0f;
    [HideInInspector]
    public Camera Camera;

    PlayerController Controller;
    public void GetPlayer(GameObject ParsedPlayer)
    {
        Controller = ParsedPlayer.gameObject.GetComponent<PlayerController>();
    }
    private void Awake()
    {
        Camera = this.gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        //Camera
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        yRotation -= mouseY;
        xRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        this.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
        //Movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = transform.position + (transform.forward * fastflyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = transform.position + (-transform.forward * fastflyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = transform.position + (-transform.right * fastflyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = transform.position + (transform.right * fastflyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position = transform.position + (transform.up * fastflyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.position = transform.position + (-transform.up * fastflyingspeed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = transform.position + (transform.forward * flyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = transform.position + (-transform.forward * flyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = transform.position + (-transform.right * flyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = transform.position + (transform.right * flyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position = transform.position + (transform.up * flyingspeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.position = transform.position + (-transform.up * flyingspeed * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.F3))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Controller.Playercontrol_enabled = true;
                Controller.Cameracontrol_enabled = true;
                Destroy(this.gameObject);
            }
        }
    }
}
