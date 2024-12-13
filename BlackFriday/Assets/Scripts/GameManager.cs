using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float maxtime = 180;
    public float timer;
    public float timer2;
    public int score;
    public bool isSecondPhase;
    public bool leftCheckout;
    public bool stageCleared;
    public bool stageFailed;

    [SerializeField]
    private TextMeshProUGUI screenTimer;
    [SerializeField]
    private TextMeshProUGUI statText;
    [SerializeField]
    private TextMeshProUGUI screenAmmo;
    private PlayerMovementScript player;
    private GunScript gun;
    [SerializeField]
    private GameObject door;

    private void Start()
    {
        door.SetActive(true);
        timer = maxtime;
        player = FindObjectOfType<PlayerMovementScript>();
        gun = FindObjectOfType<GunScript>();
        isSecondPhase = false;
        leftCheckout = true;
        stageCleared = false;
        stageFailed = false;
    }

    private void Update()
    {
        // Updates the stat manager
        UpdateStatManager();

        // Updates ammo amount
        screenAmmo.text = gun.ammo.ToString() + " / " + gun.maxAmmo.ToString();

        // massive 1-liner
        statText.text = "Health: " + player.health.ToString() + "\nSpeed: " + player.speed.ToString() + "\nJump Power: " + player.jumpForce.ToString() + "\nDamage: " + gun.damage.ToString() + "\nCooldown: " + gun.cooldown.ToString();
        if (!stageCleared && !stageFailed)
        {
            
            if (!isSecondPhase)
            {
                // Phase 1
                // During phase one, you have to collect items to take to the register.
                // Enemies will spawn here, and you will have to fend them off.
                // You can go anywhere to find things.
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    double convert = System.Math.Round(timer, 1);
                    screenTimer.text = convert.ToString();
                }
                    

                if (timer <= 0)
                {
                    Debug.Log("TIME IS UP BIG BOY");
                    stageFailed = true;
                }


                timer2 = player.GetItemsCollected() * 10;
            }
            else
            {
                // Phase 2
                // During phase two, you MUST stay in the boxed area in the checkout lane.
                // You have a timer that must be completed BASED ON however many items you picked up.
                // No shoplifting!! Btw, enemies are 2x stronger here.

                // Sets the score from the first half
                score = player.GetItemsCollected() * 100;


                if (!leftCheckout)
                {
                    timer2 -= Time.deltaTime;
                    double convert = System.Math.Round(timer2, 1);
                    screenTimer.text = convert.ToString();
                }
                    

                if (timer2 <= 0)
                {
                    Debug.Log("Stage Cleared!!!!");
                    score += player.GetItemsCollected() * 15;
                    stageCleared = true;
                    door.SetActive(false);
                }
            }
        }
    }

    private void UpdateStatManager()
    {
        PlayerStatManager.playerProfile.health = player.health;
        PlayerStatManager.playerProfile.speed = player.speed;
        PlayerStatManager.playerProfile.jump = player.jumpForce;
        PlayerStatManager.playerProfile.cooldown = gun.cooldown;
        PlayerStatManager.playerProfile.damage = gun.damage;
    }
}
