using System.Collections.Generic;
using UnityEngine;

public class ObjectDatabase : MonoBehaviour
{
    public static ObjectDatabase Instance { get; private set; }

    public Item[] ItemsInInventory = new Item[4];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        // Add item to inventory if there's space

        bool inventoryFull = true;

        for (int i = 0; i < ItemsInInventory.Length; i++)
        {
            if (ItemsInInventory[i] == null || ItemsInInventory[i].ItemName == "")
            {
                ItemsInInventory[i] = item;
                inventoryFull = false;
                return;
            }
        }

        if (inventoryFull)
        {
            //logic to replace one of the items instead

        }
    }

    public Item GetItemByName(string itemName)
    {
        foreach (Item item in ItemsInInventory)
        {
            if (item.ItemName == itemName)
            {
                return item;
            }
        }
        Debug.Log("Item not found");
        return null;
    }

    public void UpgradeItem(int itemIndex, int upgradeLevel = 1)
    {
        Item itemToUpgrade = ItemsInInventory[itemIndex];
        Item upgradedItem = RandomGenerationUtility.UpgradeItem(itemToUpgrade, upgradeLevel);
        ItemsInInventory[itemIndex] = upgradedItem;
    }

    public void UpgradeItem(Item itemToUpgrade, int upgradeLevel = 1)
    {
        int itemIndex = GetIndexByItem(itemToUpgrade);
        Item upgradedItem = RandomGenerationUtility.UpgradeItem(itemToUpgrade);
        ItemsInInventory[itemIndex] = upgradedItem;
    }

    public int GetIndexByItem(Item itemToGet)
    {
        for (int i = 0; i < ItemsInInventory.Length; i++)
        {
            if (ItemsInInventory[i].ItemName == itemToGet.ItemName)
            {
                return i;
            }
        }
        return -1;
    }
}
