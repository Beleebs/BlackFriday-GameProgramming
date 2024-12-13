using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementScript player;

    // Player picks up an item
    public void ApplyItemEffect(ItemBuff itemBuff)
    {
        // Applys buff
        Debug.Log("Sending to itemBuff");
        itemBuff.ApplyItemBuff(player);
    }
}
