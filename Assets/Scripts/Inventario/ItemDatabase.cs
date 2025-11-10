// ItemDatabase.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> allItems = new List<ItemData>();

    public ItemData GetItemByName(string name)
    {
        foreach (ItemData item in allItems)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }

    public ItemData GetItemByReference(ItemData item)
    {
        // Útil si comparas por referencia
        return allItems.Contains(item) ? item : null;
    }
}