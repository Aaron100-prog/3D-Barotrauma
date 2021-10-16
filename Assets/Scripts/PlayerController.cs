using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    [Header("Camera Movement")]
    public float sensitivity = 180f;
    [HideInInspector]
    private float yRotation = 0f;
    private float xRotation = 0f;
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

    private Transform GroundCheck;
    public LayerMask GroundMask;
    public float GroundDistance = 0.4f;
    private bool onGround;
    private bool inside;
    public LayerMask HullMask;

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

    void Start()
    {
        Camera = this.gameObject.GetComponentInChildren<Camera>();
        Controller = this.gameObject.GetComponent<CharacterController>();
        GroundCheck = this.transform.Find("Groundcheck");
        interactionHoldGO.SetActive(false);
        interactionClickGO.SetActive(false);
        Text.text = "";
    }

    void Update()
    {
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

                    interactionHoldGO.SetActive(interactable.interactiontype == Interactable.Interactiontype.HOLD);
                    interactionClickGO.SetActive(interactable.interactiontype == Interactable.Interactiontype.CLICK);
                    interactionClickGO.SetActive(interactable.interactiontype == Interactable.Interactiontype.LADDER);
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
                float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
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
            }
        }
        else
        {
            if (Cameracontrol_enabled == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
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
                        GameObject Freecam = Instantiate(FreecamPrefab, this.transform.position, Quaternion.identity) as GameObject;
                        Playercontrol_enabled = false;
                        Cameracontrol_enabled = false;
                        Freecam.SendMessage("GetPlayer", this.gameObject);
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

        Camera.transform.Rotate(Vector3.up * this.transform.localRotation.x);
        this.transform.SetParent(HullParent.transform.parent.parent);
        StartCoroutine("ResetRotation");
    }

    public void ExitHull()
    {
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

                    if (interactable.UseX() == true)
                    {
                        this.transform.position = new Vector3 (interactable.OBJECT_ForcePosition().x, this.transform.position.y, this.transform.position.z);
                    }
                    if (interactable.UseY() == true)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, interactable.OBJECT_ForcePosition().y, this.transform.position.z);
                    }
                    if (interactable.UseZ() == true)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, interactable.OBJECT_ForcePosition().z);
                    }

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
