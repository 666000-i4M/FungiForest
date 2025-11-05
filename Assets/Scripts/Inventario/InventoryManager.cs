using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventario")]
    public List<ItemData> items = new List<ItemData>(); // ← Cambiado

    [Header("UI")]
    public GameObject inventoryPanel;
    public Transform slotsParent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void AddItem(ItemData itemData) // ← Cambiado
    {
        if (!items.Contains(itemData))
        {
            items.Add(itemData);
            UpdateInventoryUI();
        }
    }

    public void UseItem(ItemData itemData) // ← Cambiado
    {
        if (items.Contains(itemData))
        {
            items.Remove(itemData);
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        foreach (ItemData item in items) // ← Cambiado
        {
            GameObject slot = new GameObject("Slot");
            slot.transform.SetParent(slotsParent, false);

            var image = slot.AddComponent<UnityEngine.UI.Image>();
            image.sprite = item.icon; // ← item.icon sigue funcionando
            image.preserveAspect = true;

            var button = slot.AddComponent<UnityEngine.UI.Button>();
            int index = items.IndexOf(item);
            button.onClick.AddListener(() => UseItem(items[index]));
        }
    }
}