using UnityEngine;
using System.Collections.Generic; // ← IMPORTANTE


public class InteractableNPC : MonoBehaviour
{
    public DialogueData dialogueData;
    public float interactDistance = 10f; // distancia aumentada para pruebas
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Solo permitir interacción dentro de la distancia
        if (distance <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("H presionada cerca del NPC");

                // Asignar NPC actual para centrar la cámara
                DialogueManager.Instance.SetCurrentNPC(transform);

                // Abrir diálogo
                DialogueManager.Instance.OpenDialogue(dialogueData);
            }
        }
    }
}
