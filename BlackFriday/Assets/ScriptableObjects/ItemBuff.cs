using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    HP,
    Speed,
    Jump,
    Damage,
    GunBulletAmount,
    GunCooldown,
    GunMagSize
}

[CreateAssetMenu(fileName = "Item Buff", menuName = "Item")]
public class ItemBuff : ScriptableObject
{
    // Prefab instance/model
    public GameObject buffPrefab;
    // What the item name will be
    public string itemName;
    // Corresponding with the itemName, This is the buff type that will be applied
    public StatType buff;
    // The percentage amount that will be buffed
    public float buffAmountFloat;
    // The value amount that will be buffed
    public int buffAmountInt;

    // Generic function to apply any sort of item buff to the player.
    public void ApplyItemBuff(PlayerMovementScript player)
    {
        // Using a switch statement with the buff that is being applied,
        // we can add this to the player script.
        switch (buff)
        {
            case StatType.HP:
                player.IncreaseStatsInts(StatType.HP, buffAmountInt);
                break;
            case StatType.Speed:
                player.IncreaseStatsFloats(StatType.Speed, buffAmountFloat);
                break;
            case StatType.Jump:
                player.IncreaseStatsFloats(StatType.Jump, buffAmountFloat);
                break;
            case StatType.Damage:
                player.IncreaseStatsFloats(StatType.Damage, buffAmountFloat);
                break;
            case StatType.GunBulletAmount:
                player.IncreaseStatsInts(StatType.GunBulletAmount, buffAmountInt);
                break;
            case StatType.GunCooldown:
                player.IncreaseStatsFloats(StatType.GunCooldown, buffAmountFloat);
                break;
            case StatType.GunMagSize:
                player.IncreaseStatsInts(StatType.GunMagSize, buffAmountInt);
                break;
        }
    }
}
