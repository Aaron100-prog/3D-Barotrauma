using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void StartScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Spiel geschlossen");
    }
}
