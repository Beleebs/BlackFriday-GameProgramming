using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    // This script will take you to either the lobby or to the main menu.
    // Depending if you complete the level, die, or just want to quit.

    // used for determining if the game is won or lost.
    private GameManager gm;

    private void Start()
    {
        // finds the game manager
        gm = FindObjectOfType<GameManager>();
    }

    private void LeaveSceneResult()
    {
        if (gm.stageCleared)
        {
            // takes the remainder of the timer
            float phaseOneTimer = gm.maxtime - gm.timer;
            // Difficulty Calculation
            float difficultyIncrease = IterationManager.Instance.difficulty + phaseOneTimer;
            PlayerPrefs.SetFloat("DifficultyIncrease", difficultyIncrease);
            SceneManager.LoadScene("LobbyScene");
        }
        else if (gm.stageFailed)
        {
            // Stage failed!!!
            Debug.Log("Stage Failed");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void QuitRun()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter(Collider other)
    {
        LeaveSceneResult();
    }
}
