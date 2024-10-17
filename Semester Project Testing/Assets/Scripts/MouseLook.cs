using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Sensitivity for X and Y axis
    [Range(50, 2000)]
    public float sensX = 500;

    [Range(50, 2000)]
    public float sensY = 509;

    // The current camera position
    public Transform pos;

    // Rotation variables
    float xRot;
    float yRot;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Gets input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        // Rotates Camera
        xRot -= mouseY;
        yRot += mouseX;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        pos.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
