using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementScript : MonoBehaviour
{
    // Player Stats
    public int health = 100;
    public float speed = 3.2f;
    public float jumpForce = 32;

    // Variable used for obtaining current position
    public Transform pos;

    // Input Variables
    private float xpos;
    private float zpos;
    public KeyCode jumpKey = KeyCode.Space;

    // Variables used to determine movement for calculation
    public Vector3 movement;
    public float sprintMulti = 1.3f;
    [SerializeField]
    Rigidbody rb;

    // Drag Variables
    private float playerHeight = 1.9f;
    public LayerMask whatIsGround;
    bool isGrounded;
    public float dragCoefficient = 2;

    // Jump Variables
    public float jumpCooldown;
    public float airMulti;
    bool jumpReady = true;

    // Item Stats
    public int itemsCollected;
    [SerializeField]
    private GunScript gun;
    
    void Start()
    {
        health = PlayerStatManager.playerProfile.health;
        speed = PlayerStatManager.playerProfile.speed;
        jumpForce = PlayerStatManager.playerProfile.jump;
        gun.cooldown = PlayerStatManager.playerProfile.cooldown;
        gun.damage = PlayerStatManager.playerProfile.damage;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        // Sends a raycast to the ground, checking if the player is on or close to the ground to apply drag.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        InputFunc();

        if (!isGrounded)
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
        movement = pos.forward * zpos + pos.right * xpos;

        // Moves the player (grounded)
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(movement.normalized * speed * sprintMulti, ForceMode.Impulse);
            }

            else
            {
                rb.AddForce(movement.normalized * speed, ForceMode.Impulse);
            }
        }
            

        // Moves the player (in air)
        else if (!isGrounded)
            rb.AddForce(movement.normalized * airMulti, ForceMode.Impulse);
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

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "whatIsGround")
        {
            Debug.Log("ground");
        }
    }

    // Increase Stats fed from the ItemBuff class (using floats)
    public void IncreaseStatsFloats(StatType stat, float amount)
    {
        // Similar to the itembuff switch statement
        switch (stat)
        {
            case StatType.Speed:
                speed += amount;
                itemsCollected += 1;
                break;
            case StatType.Jump:
                jumpForce += amount;
                itemsCollected += 1;
                break;
            case StatType.Damage:
                gun.damage *= (1 + amount);
                itemsCollected += 1;
                break;
            case StatType.GunCooldown:
                gun.cooldown *= amount;
                itemsCollected += 1;
                break;
            default:
                Debug.Log("Unknown stat type: " + stat);
                break;
        }
    }

    // Increase stats (using ints)
    public void IncreaseStatsInts(StatType stat, int amount)
    {
        switch (stat)
        {
            case StatType.HP:
                health += amount;
                itemsCollected += 1;
                break;
            case StatType.GunBulletAmount:
                gun.bulletAmount += amount;
                itemsCollected += 1;
                break;
            case StatType.GunMagSize:
                gun.magSize += amount;
                itemsCollected += 1;
                break;
            default:
                Debug.Log("Unknown stat type: " + stat);
                break;
        }
    }

    public int GetItemsCollected()
    {
        return itemsCollected;
    }

    public void TakeDamage(int d)
    {
        health -= d;
    }
}
