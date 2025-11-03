using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [Header("Datos de diálogo")]
    public DialogueData dialogueData;

    [Header("Configuración de interacción")]
    public float interactDistance = 3f; // Alternativa a trigger
    private bool playerInRange = false;

    private Transform player;

    void Start()
    {
        // Buscar jugador por tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("No se encontró el jugador con tag 'Player'");
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("H presionada cerca del NPC");

            // Centrar cámara en el NPC
            if (CameraFollow.Instance != null)
                CameraFollow.Instance.CenterCameraOnTarget(transform);
            else
                Debug.LogError("CameraFollow.Instance es null");

            // Abrir diálogo
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OpenDialogue(dialogueData);
            else
                Debug.LogError("DialogueManager.Instance es null");
        }

        // Opcional: alternativa sin trigger usando distancia
        if (player != null && Vector3.Distance(player.position, transform.position) <= interactDistance)
            playerInRange = true;
        else
            playerInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jugador dentro del rango del NPC");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Jugador salió del rango del NPC");
        }
    }
}
