using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    // Variable used for obtaining current position
    public Transform pos;

    // Input Variables
    private float xpos;
    private float zpos;
    public KeyCode jumpKey = KeyCode.Space;

    // Variables used to determine movement for calculation
    public Vector3 movement;
    public float speed = 10;
    
    // GetComponent() variables
    Rigidbody rb;

    // Drag Variables
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsIcy;
    bool isGrounded;
    bool isIcy;
    public float dragCoefficient = 10;

    // Jump Variables
    public float jumpForce;
    public float jumpCooldown;
    public float airMulti;
    bool jumpReady = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Sends a raycast to the ground, checking if the player is on or close to the ground to apply drag.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        isIcy = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsIcy);
        if (isIcy)
            Debug.Log("THIS SHIT ICY");

        InputFunc();

        // Apply drag
        if (isGrounded && isIcy)
        {
            rb.drag = 0; rb.angularDrag = 0;
        }
        else if (!isGrounded)
        {
            rb.drag = 0;
        }
        else if (isGrounded)
        {
            rb.drag = dragCoefficient;
        }           
    }

    private void FixedUpdate()
    {
        MovementFunc();
    }

    private void InputFunc()
    {
        // Gets x and z input
        xpos = Input.GetAxisRaw("Horizontal");
        zpos = Input.GetAxisRaw("Vertical");
        
        // Gets jumpKey input
        if (Input.GetKey(jumpKey) && isGrounded && jumpReady)
        {
            jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovementFunc()
    {
        // Calculates the movement for player
        // I'm making it carry forward momentum
        if (rb.velocity.z >= 2)
            movement = pos.forward * zpos * (rb.velocity.z / 2) + pos.right * xpos;
        else if (rb.velocity.z < 2)
            movement = pos.forward * zpos + pos.right * xpos;

        // Moves the player (grounded)
        if (isGrounded)
            rb.AddForce(movement.normalized * speed * 10, ForceMode.Force);

        // Moves the player (in air)
        else if (!isGrounded)
            rb.AddForce(movement.normalized * speed * 10 * airMulti, ForceMode.Force);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        jumpReady = true;
    }
}
