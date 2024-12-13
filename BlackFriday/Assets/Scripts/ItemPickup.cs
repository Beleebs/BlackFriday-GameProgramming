using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private ItemBuff item;
    [SerializeField]
    private ItemManager itemManager;

    private void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        if (itemManager)
        {
            Debug.Log("Found ItemManager!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            itemManager.ApplyItemEffect(item);
            Destroy(gameObject);
        }
    }
}
