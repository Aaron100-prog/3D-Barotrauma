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

    }
}
