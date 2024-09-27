using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject MainPausePanel;
    public GameObject Player;

    private void Awake()
    {
        LockCursor(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            togglepausemenu();
        }
    }
    public void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void togglepausemenu()
    {
        MainPausePanel.SetActive(!MainPausePanel.activeSelf);
        Player.GetComponent<PlayerController>().enabled = !Player.GetComponent<PlayerController>().enabled;
        Player.GetComponent<CharacterController>().enabled = !Player.GetComponent<CharacterController>().enabled;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Geschlossen");
    }
}
