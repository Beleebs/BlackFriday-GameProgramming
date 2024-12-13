using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    // Gets the player's stats and stores them in this specific profile
    public static PlayerStatManager playerProfile;
    public int health;
    public float speed;
    public float jump;
    public float cooldown;
    public float damage;

    private void Awake()
    {
        // Creates a game-wide instance
        if (playerProfile == null)
        {
            playerProfile = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CleanStats()
    {
        // sets stats back to default
        health = 100;
        speed = 3.2f;
        jump = 32;
        cooldown = 1;
        damage = 5;
    }
}
