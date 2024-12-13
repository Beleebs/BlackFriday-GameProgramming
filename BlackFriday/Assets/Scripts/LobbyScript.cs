using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviour
{
    private void Start()
    {
        IterationManager.Instance.iteration++;
        if (IterationManager.Instance.iteration > 5)
        {
            SceneManager.LoadScene("WinScreen");
        }

        // Makes sure the cursor isn't locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        float difficultyIncrease = PlayerPrefs.GetFloat("DifficultyIncrease", 1f);
        IterationManager.Instance.difficulty += difficultyIncrease;

        Debug.Log($"Iteration: {IterationManager.Instance.iteration}, Difficulty: {IterationManager.Instance.difficulty}");
    }

    // This function is activated by button press in the lobby scene
    // based on what the type is, that is the kind of structure that will generate.
    public void LoadSelectedBuilding(string type)
    {
        PlayerPrefs.SetString("Selected Building", type);
        SceneManager.LoadScene("GameplayScene");
    }
}
