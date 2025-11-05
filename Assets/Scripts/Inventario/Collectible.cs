using UnityEngine;

public class Collectible : MonoBehaviour
{
    public ItemData itemData; // ← Cambiado

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}