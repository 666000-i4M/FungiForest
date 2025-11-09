// DoorPuzzleTrigger.cs
using UnityEngine;

public class DoorPuzzleTrigger : MonoBehaviour
{
    [Header("Configuración")]
    public float interactDistance = 2f;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        // Verificar que el jugador exista
        if (PlayerController.Instance == null) return;

        float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distance <= interactDistance && Input.GetKeyDown(interactKey))
        {
            // Buscar el PuzzleManager (solo una vez si quieres optimizar)
            var puzzleManager = FindObjectOfType<PuzzleManager3D>();
            if (puzzleManager != null)
                puzzleManager.OpenPuzzle();
            else
                Debug.LogWarning("No se encontró PuzzleManager3D en la escena");
        }
    }
}