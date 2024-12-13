using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textDisplay;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IterationManager.Instance != null)
        {
            // Changes the text on the screen to make it represent iteration and simplified difficulty
            double convert = System.Math.Round(IterationManager.Instance.difficulty, 3);
            textDisplay.text = "Building Number: " + IterationManager.Instance.iteration.ToString() + "\nDifficulty: " + convert.ToString();
        }
        else
        {
            Debug.LogWarning("IterationManager is not found!");
        }
    }
}
