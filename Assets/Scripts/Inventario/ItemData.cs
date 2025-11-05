using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
