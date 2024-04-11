using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Sprite ItemImage; //Upgradeable
    public String ItemName;
    public String ItemDescription;
    public List<Ability> Abilities; //Upgradevalue 
    public List<PreviousItem> ItemHistory;
    public String UpgradeText;


    public void AddItemUpgrade(Item upgradedItem)
    {
         PreviousItem previousItem = new PreviousItem
        {
            PrevItemImage = ItemImage,
            PrevItemName = ItemName,
            PrevItemDescription = ItemDescription,
            PrevAbilities = Abilities
        } ;
        if (ItemHistory == null)
        {
            ItemHistory = new List<PreviousItem>();
        }
        ItemHistory.Add(previousItem);

        ItemImage = upgradedItem.ItemImage;
        ItemName = upgradedItem.ItemName;
        ItemDescription = upgradedItem.ItemDescription;
        Abilities = upgradedItem.Abilities;
    }
}
