using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckoutScript : MonoBehaviour
{
    GameManager gm;
    [SerializeField]
    private GameObject prompt;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // prevents multiple inputs
            if (!gm.isSecondPhase)
            {
                // ask for input
                if (Input.GetKey(KeyCode.Y))
                {
                    // disable ui
                    Debug.Log("Yes");
                    gm.isSecondPhase = true;
                    prompt.SetActive(false);
                }

                if (Input.GetKey(KeyCode.N))
                {
                    Debug.Log("No");
                    prompt.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // disable ui
            Debug.Log("Player left the area");
            gm.leftCheckout = true;
            prompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // prompt/enable ui
            Debug.Log("Player entered the area");
            gm.leftCheckout = false;
            if (!gm.isSecondPhase)
            {
                prompt.SetActive(true);
            }
        }
    }


}
