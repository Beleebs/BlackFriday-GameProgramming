using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IterationManager : MonoBehaviour
{
    // this instance is used in a singleton to make sure that
    // the game doesn't mess up progress based on scene swapping
    // or if it's restarted.
    public static IterationManager Instance;
    public int iteration = 1;
    public float difficulty = 1.005f;

    private void Awake()
    {
        // first time IterationManager is opened
        if (Instance == null)
        {
            // Keeps the instance loaded while the gameplay goes on
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another pops up at any time, it will be discarded.
            Destroy(gameObject);
        }
    }
}

