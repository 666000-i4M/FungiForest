using UnityEngine;

public class InteractWithBela : MonoBehaviour
{
    public BakingGameController bakingGame;
    public int requiredItems = 2; // Cantidad de ítems necesarios

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verifica si tiene 2 ítems en el inventario
            if (InventoryManager.Instance.items.Count >= requiredItems)
            {
                bakingGame.StartGame();
            }
            else
            {
                Debug.Log("Necesitas al menos " + requiredItems + " ingredientes para ayudar a Bela.");
            }
        }
    }
}