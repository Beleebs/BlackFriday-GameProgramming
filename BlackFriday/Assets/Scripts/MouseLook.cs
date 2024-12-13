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

    // Camera Velocity variables
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    private float SelectedFOV = 80;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraLook();
        ChangeFOVFromVelocity();
    }

    private void ChangeFOVFromVelocity()
    {
        if (rb.velocity == new Vector3(0,0,0))
        {
            Camera.main.fieldOfView = SelectedFOV;
        }
        else
        {
            Camera.main.fieldOfView = ((rb.velocity.magnitude) / 2 + SelectedFOV);
        }
    }

    private void CameraLook()
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
