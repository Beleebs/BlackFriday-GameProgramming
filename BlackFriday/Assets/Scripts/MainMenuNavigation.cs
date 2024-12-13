using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    private void Start()
    {
        // Makes sure the cursor isn't locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Clean slate for the game.
        PlayerPrefs.DeleteAll();
        if (IterationManager.Instance != null)
        {
            IterationManager.Instance.iteration = 0;
            IterationManager.Instance.difficulty = 1.05f;
        }

        if (PlayerStatManager.playerProfile != null)
        {
            PlayerStatManager.playerProfile.CleanStats();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        // Nighty night game zzzzzzzzzzzz
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
