// Collectible.cs
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public ItemData itemData; // ← Asigna este ítem en el Inspector

    private void Awake()
    {
        // Verificación de seguridad al cargar
        if (itemData == null)
        {
            Debug.LogError("ItemData no asignado en el coleccionable: " + name, this);
        }

        // Asegurar que el collider esté configurado como trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError("El coleccionable " + name + " no tiene un Collider. Añade un BoxCollider, SphereCollider, etc.", this);
        }

        // Asegurar que tenga Rigidbody (necesario para que OnTrigger funcione)
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        // Solo el jugador puede recoger ítems
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                InventoryManager.Instance.AddItem(itemData);
                Destroy(gameObject); // Elimina el coleccionable de la escena
            }
            else
            {
                Debug.LogWarning("Intento de recoger un ítem nulo desde: " + name);
                Destroy(gameObject); // Aún así, elimina el objeto para no dejar basura
            }
        }
    }*/

        private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión con: " + other.name); // ← Para ver qué objeto colisionó

        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}