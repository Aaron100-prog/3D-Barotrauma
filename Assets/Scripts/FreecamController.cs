using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreecamController : MonoBehaviour
{
    PlayerController Controller;
    public void GetPlayer(GameObject ParsedPlayer)
    {
        Controller = ParsedPlayer.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
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
