﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    

    [Header("Camera Movement")]
    public float sensitivity = 2f;
    [HideInInspector]
    private float yRotation = 0f;
    private float xRotation = 0f;
    public bool Cameracontrol_enabled = true;
    [HideInInspector]
    public Camera Camera;
    private bool Zoomed = false;
    public GameObject WaterPlane;

    [Header("Player Movement")]
    public bool Playercontrol_enabled = true;
    [HideInInspector]
    public CharacterController Controller;
    [HideInInspector]
    private Vector3 velocity;
    public float walkspeed = 1.5f;
    public float runspeed = 4f;
    public float fallspeed = -10f;

    private Transform GroundCheck;
    public LayerMask GroundMask;
    public float GroundDistance = 0.4f;
    private bool onGround;
    private bool inside;
    public LayerMask HullMask;

    public bool Crouched;

    [Header("Swimming")]
    public bool swimming = false;
    public float downdrifting = -0.25f;

    private bool ragdolled = false;

    public TMPro.TextMeshProUGUI Text;
    public UnityEngine.UI.Image InteractionProgress;
    public GameObject interactionHoldGO;
    public GameObject interactionClickGO;

    public GameObject FreecamPrefab;

    public bool altlookactiv;
    Quaternion savedaltlook;

    private bool OnLadder;

    [Header("Access")]
    public string[] PlayerAccess;

    [Header("Stats")]
    public float Oxygen = 100f;

    [Header("Debug")]
    [HideInInspector]
    public GameObject DebugMenu;
    [SerializeField]
    private bool UseOxygen = true;
    private bool MaskOn = false;

    void Start()
    {
        Camera = this.gameObject.GetComponentInChildren<Camera>();
        Controller = this.gameObject.GetComponent<CharacterController>();
        GroundCheck = this.transform.Find("Groundcheck");
        interactionHoldGO.SetActive(false);
        interactionClickGO.SetActive(false);
        Text.text = "";
        DebugMenu = GameObject.FindWithTag("Canvas").transform.Find("Debug").gameObject;
    }

    void Update()
    {
        if (UseOxygen)
        {
            Collider[] hitcollider = Physics.OverlapSphere(this.transform.position, 0f, HullMask, QueryTriggerInteraction.Collide);
            if (!MaskOn && hitcollider.Length != 0)
            {
                Hull hull = hitcollider[0].gameObject.GetComponent<Hull>();
                if(hull != null)
                {
                    //Versuchen Sauerstoff aus Hülle zunehmen
                    float difference = hull.OxygenInHull - 2f;
                    if(difference >= 0)
                    {
                        hull.OxygenInHull -= 2f * Time.deltaTime;
                        if (Oxygen < 100 && hull.OxygenInHull > 0) //Wenn der Charakter weniger als 100 Sauerstoff hat, zusätzlich Sauerstoff aus Hülle entnehmen
                        {
                            float difference2 = hull.OxygenInHull - 5f;
                            if (difference2 >= 0)
                            {
                                hull.OxygenInHull -= 5f * Time.deltaTime;
                                Oxygen += 5f * Time.deltaTime;
                            }
                            if(Oxygen > 100) //Wenn der Charakter mehr als 100 Sauerstoff hat, zusätzlichen Sauerstoff von Charakter nehmen und der Hülle hinzufügen
                            {
                                float difference3 = Oxygen - 100f;
                                Oxygen -= difference3;
                                hull.OxygenInHull += difference3;
                            }
                        }

                    }
                    else //Wenn nicht genügend Sauerstoff in der Hülle ist, verbleibenden Sauerstoff aus Hülle nehmen und restlichen vom Charakter
                    {
                        hull.OxygenInHull -= 2f + difference;
                        Oxygen += difference * Time.deltaTime;
                    }
                }
                else //Wenn die Hülle keinen Hüllen Script besitzt, Sauerstoff direkt vom Charakter nehmen
                {
                    Debug.Log(hitcollider[0].gameObject.name + " besitzt keinen Hüllen Script!");
                    Oxygen -= 2f * Time.deltaTime;
                }
                
            }
            else //Wenn der Charakter sich nicht in einer Hülle befindet, Sauerstoff direkt vom Charakter nehmen
            {
                Oxygen -= 2f * Time.deltaTime;
            }
        }
        if (!OnLadder)
        {
            Collider[] hitcollider = Physics.OverlapSphere(this.transform.position, 0f, HullMask, QueryTriggerInteraction.Collide); //TODO: OverlapCapsule benutzen damit der gesamten Charakter geprüft wird, und so niemals mit Arm/Bein/Kopf außerhalb einer Hülle ist, aber noch behandelt wird als wäre er in einer hülle.
            if (hitcollider.Length != 0 && swimming == true)
            {
                EnterHull(hitcollider[0]);
            }
            else if (hitcollider.Length == 0 && swimming == false)
            {
                ExitHull();
            }
            if(DebugMenu.activeSelf && hitcollider.Length != 0)
            {
                Hull hull = hitcollider[0].gameObject.GetComponent<Hull>();
                if(hull != null)
                {
                    DebugMenu.transform.Find("Hull Info").Find("Hull Name Info").GetComponent<TextMeshProUGUI>().text = hull.gameObject.name;
                    DebugMenu.transform.Find("Hull Info").Find("Hull Oxygen Info").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(hull.OxygenInHull / hull.HullVolume*100).ToString() + "%";
                    DebugMenu.transform.Find("Hull Info").Find("Hull Pressure Info").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(hull.PressureInHull / 100 * 100).ToString() + "%";
                    DebugMenu.transform.Find("Hull Info").Find("Hull Water Info").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(hull.WaterInHull / hull.HullVolume * 100).ToString() + "%";
                }
                else
                {
                    DebugMenu.transform.Find("Hull Info").Find("Hull Name Info").GetComponent<TextMeshProUGUI>().text = "Hull Script fehlt!";
                }
            }
        }
        else
        {
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 2;
            transform.Translate(0f, y, 0f);
            Vector3 clampPosition = transform.localPosition;
            clampPosition.y = Mathf.Clamp(clampPosition.y, -1, 1);
            transform.localPosition = clampPosition;
        }
        if(!OnLadder)
        {
            Ray ray = Camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            bool success = false;
            if (Physics.Raycast(ray, out hit, 4f))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    PerformInteraction(interactable);
                    Text.text = interactable.GetDescription();
                    success = true;

                    switch (interactable.interactiontype)
                    {
                        case Interactable.Interactiontype.CLICK:
                            interactionClickGO.SetActive(true);
                            break;
                        case Interactable.Interactiontype.HOLD:
                            interactionHoldGO.SetActive(true);
                            break;
                        case Interactable.Interactiontype.LADDER:
                            interactionClickGO.SetActive(true);
                            break;
                        case Interactable.Interactiontype.BUTTON:
                            interactionClickGO.SetActive(true);
                            break;
                    }
                }

                if (!success)
                {
                    Text.text = "";
                    interactionHoldGO.SetActive(false);
                    interactionClickGO.SetActive(false);
                }
            }
        }
        else if(OnLadder && Input.GetKeyDown(KeyCode.F))
        {
            this.transform.SetParent(this.transform.parent.parent.parent, true);
            Playercontrol_enabled = true;
            Controller.enabled = true;
            OnLadder = false;
        }

        if(swimming == false)
        {
            if (Cameracontrol_enabled == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
                if(!altlookactiv)
                {
                    yRotation -= mouseY;
                    yRotation = Mathf.Clamp(yRotation, -90f, 90f);
                    Camera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
                    this.transform.Rotate(Vector3.up * mouseX);
                }
                else
                {
                    yRotation -= mouseY;
                    yRotation = Mathf.Clamp(yRotation, -80f, 90f);
                    xRotation += mouseX;
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                    Camera.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
                }
                

                if(Input.GetMouseButtonDown(2))
                {
                    Zoomed = true;
                }
                if(Input.GetMouseButtonUp(2))
                {
                    Zoomed = false;
                }
                if (Zoomed)
                {
                    if (System.Math.Round(Camera.fieldOfView, 2) != 20)
                    {
                        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 20, Time.deltaTime * 2);
                        if (System.Math.Round(Camera.fieldOfView, 2) == 20)
                        {
                            Camera.fieldOfView = 20;
                        }
                    }
                }
                else
                {
                    if(System.Math.Round(Camera.fieldOfView, 2) != 60)
                    {
                        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 60, Time.deltaTime * 2);
                        if (System.Math.Round(Camera.fieldOfView, 2) == 60)
                        {
                            Camera.fieldOfView = 60;
                        }
                    }
                    
                }
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    altlookactiv = true;
                    yRotation = 0f;
                    xRotation = 0f;
                }
                if (Input.GetKeyUp(KeyCode.LeftAlt))
                {
                    altlookactiv = false;
                    Camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    yRotation = 0f;
                    xRotation = 0f;
                }
                if (Input.GetKeyDown(KeyCode.F12))
                {
                    ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + System.DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss") + ".png");
                    Debug.Log("Screenshot " + Application.dataPath + "/Screenshots/" + System.DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss") + ".png");
                }
                if(Input.GetKey(KeyCode.F3))
                {
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject Freecam = Instantiate(FreecamPrefab, this.transform.position, Quaternion.identity) as GameObject;
                        Playercontrol_enabled = false;
                        Cameracontrol_enabled = false;
                        Freecam.SendMessage("GetPlayer", this.gameObject);
                    }
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        DebugMenu.SetActive(!DebugMenu.activeSelf);
                    }
                }
            }
            if (Playercontrol_enabled)
            {
                onGround = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
                //Debug.Log(velocity.y + " " + onGround);
                if (onGround && velocity.y < 0)
                {
                    velocity.y = -2f;
                }
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

                if (Input.GetKey(KeyCode.Q))
                {
                    Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 35);
                    Quaternion targetRotation = Quaternion.Euler(direction);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 3);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -35);
                    Quaternion targetRotation = Quaternion.Euler(direction);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 3);
                }
                else
                {
                    if(System.Math.Round(this.transform.eulerAngles.z, 2) != 0)
                    {
                        Vector3 direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                        Quaternion targetRotation = Quaternion.Euler(direction);
                        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 4);
                        if(System.Math.Round(this.transform.eulerAngles.z, 2) == 0)
                        {
                            this.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                        }
                    }
                }
                
                velocity.y += fallspeed * Time.deltaTime;
                Controller.Move(velocity * Time.deltaTime);

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Crouched = !Crouched;
                    if (Crouched)
                    {
                        walkspeed = 0.5f;
                        runspeed = 1.5f;
                        Controller.height = 1;
                    }
                    else
                    {
                        walkspeed = 1.5f;
                        runspeed = 4;
                        Controller.height = 2;
                    }
                }
                if (Crouched)
                {
                    if (System.Math.Round(Camera.transform.localPosition.y, 2) != 0.15f)
                    {
                        Camera.transform.localPosition = new Vector3(0f, Mathf.Lerp(Camera.transform.localPosition.y, 0.15f, Time.deltaTime * 10), 0f);
                        if (System.Math.Round(Camera.transform.localPosition.y, 2) == 0.15f)
                        {
                            Camera.transform.localPosition = new Vector3(0f, 0.15f, 0f);
                        }
                    }
                }
                else
                {
                    if (System.Math.Round(Camera.transform.localPosition.y, 2) != 0.75f)
                    {
                        Camera.transform.localPosition = new Vector3(0f, Mathf.Lerp(Camera.transform.localPosition.y, 0.75f, Time.deltaTime * 10), 0f);
                        if (System.Math.Round(Camera.transform.localPosition.y, 2) == 0.75f)
                        {
                            Camera.transform.localPosition = new Vector3(0f, 0.75f, 0f);
                        }
                    }
                }
            }
        }
        else
        {
            if (Cameracontrol_enabled == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
                if (!altlookactiv)
                {
                    this.transform.Rotate(Vector3.left * mouseY + Vector3.up * mouseX);
                }
                else
                {
                    yRotation -= mouseY;
                    yRotation = Mathf.Clamp(yRotation, -80f, 90f);
                    xRotation += mouseX;
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                    Camera.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
                }
                

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
                    if (System.Math.Round(Camera.fieldOfView, 2) != 20)
                    {
                        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 20, Time.deltaTime * 2);
                        if (System.Math.Round(Camera.fieldOfView, 2) == 20)
                        {
                            Camera.fieldOfView = 20;
                        }
                    }
                }
                else
                {
                    if (System.Math.Round(Camera.fieldOfView, 2) != 60)
                    {
                        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 60, Time.deltaTime * 2);
                        if (System.Math.Round(Camera.fieldOfView, 2) == 60)
                        {
                            Camera.fieldOfView = 60;
                        }
                    }

                }
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    altlookactiv = true;
                    yRotation = 0f;
                    xRotation = 0f;
                }
                if (Input.GetKeyUp(KeyCode.LeftAlt))
                {
                    altlookactiv = false;
                    Camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    yRotation = 0f;
                    xRotation = 0f;
                }
                if (Input.GetKeyDown(KeyCode.F12))
                {
                    ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + System.DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss") + ".png");
                    Debug.Log("Screenshot " + Application.dataPath + "/Screenshots/" + System.DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss") + ".png");
                }
                if (Input.GetKey(KeyCode.F3))
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject Freecam = Instantiate(FreecamPrefab, transform.position, Quaternion.identity) as GameObject;
                        Playercontrol_enabled = false;
                        Cameracontrol_enabled = false;
                        Freecam.SendMessage("GetPlayer", this.gameObject);
                    }
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        DebugMenu.SetActive(!DebugMenu.activeSelf);
                    }
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
    }

    public void EnterHull(Collider HullParent)
    {
        swimming = false;
        WaterPlane.SetActive(false);

        Camera.transform.Rotate(Vector3.up * this.transform.localRotation.x);
        this.transform.SetParent(HullParent.transform.parent.parent);
        StartCoroutine("ResetRotation");
    }

    public void ExitHull()
    {
        WaterPlane.SetActive(true);
        swimming = true;

        this.transform.localRotation = Quaternion.Euler(Camera.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        Camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.transform.SetParent(null);

        StopCoroutine("ResetRotation");
    }

    void PerformInteraction(Interactable interactable)
    {
        switch (interactable.interactiontype)
        {
            case Interactable.Interactiontype.CLICK:
                if(Input.GetKeyDown(KeyCode.F))
                {
                        interactable.Interact();
                        
                }
                break;
            case Interactable.Interactiontype.HOLD:
                if (Input.GetKey(KeyCode.F))
                {
                    interactable.IncreaseHOLDTime();
                    if(interactable.GetHOLDTime() > 1f)
                    {
                        interactable.Interact();
                        interactable.ResetHOLDTime();
                    }
                }
                else
                {
                    interactable.ResetHOLDTime();
                }
                InteractionProgress.fillAmount = interactable.GetHOLDTime();
                break;
            case Interactable.Interactiontype.LADDER:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactable.Interact();
                    this.transform.SetParent(interactable.transform.parent.transform, true);
                    Playercontrol_enabled = false;
                    Controller.enabled = false;
                    OnLadder = true;
                    Debug.Log(transform.localPosition);
                    Debug.Log(interactable.OBJECT_ForcePosition());
                    
                    this.transform.localPosition = new Vector3 (interactable.OBJECT_ForcePosition().x, transform.localPosition.y, interactable.OBJECT_ForcePosition().z);
                    
                    Debug.Log(transform.localPosition);
                }
                break;
            case Interactable.Interactiontype.BUTTON:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactable.SendMessage("GetPlayerAccess", PlayerAccess);
                    interactable.Interact();
                }
                break;
        }
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
