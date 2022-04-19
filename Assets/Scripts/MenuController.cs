using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public bool menuopen = false;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ToggleMenu()
    {
        if(menuopen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            menuopen = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            menuopen = true;
        }
        
    }
}
