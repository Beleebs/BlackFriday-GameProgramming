using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Relation to the player's current position
    [SerializeField]
    public PlayerMovementScript player;
    public float detectionRange = 300;
    public float attackRange = 2;
    public float speed;
    public int damage;
    public float health;

    public float attackCooldown = 1.5f;
    public float attackTimer = 0;

    private void Start()
    {
        AssignStats();
        player = FindObjectOfType<PlayerMovementScript>();
    }

    private void Update()
    {
        // if the health is ever 0 or under, it DIES
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            // The enemy faces the player
            transform.LookAt(player.transform);
            // if the distance is not close enough to be in attack range,
            // the enemy will instead move towards the player.
            if (distanceToPlayer > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else
            {
                Attack();
            }
        }

        // Activated attack cooldown
        attackTimer -= Time.deltaTime;
    }

    private void AssignStats()
    {
        if (IterationManager.Instance != null)
        {
            health = 15 * ((1 + IterationManager.Instance.difficulty) / 100);
            speed = 3 * ((1 + IterationManager.Instance.difficulty) / 100);
            damage = 8 * IterationManager.Instance.iteration;
        }
        else
        {
            health = 10;
            speed = 5;
            damage = 10;
        }
    }

    private void Attack()
    {
        // ONLY activates when the attack timer is 0 (or less)
        if (attackTimer <= 0f)
        {
            player.TakeDamage(damage);
            attackTimer = attackCooldown;
        }
    }

    public void TakeDamage(float d)
    {
        health -= d;
    }
}
