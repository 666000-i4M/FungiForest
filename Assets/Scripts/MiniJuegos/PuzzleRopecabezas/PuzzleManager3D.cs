// PuzzleManager3D.cs
using UnityEngine;
using System.Linq;

public class PuzzleManager3D : MonoBehaviour
{
    public static PuzzleManager3D Instance;

    public GameObject puzzleRoot;        // ← ahora es GameObject (más simple)
    public Transform doorTransform;
    public float doorRaiseAmount = 2f;
    public float doorRaiseSpeed = 2f;
    public KeyCode exitKey = KeyCode.Escape;

    private bool isPuzzleActive = false;
    private bool isDoorRaised = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if (puzzleRoot != null)
            puzzleRoot.SetActive(false);
    }

    void Update()
    {
        if (isPuzzleActive && Input.GetKeyDown(exitKey))
        {
            ClosePuzzle();
        }
    }

   public void OpenPuzzle()
{
    if (isDoorRaised) return;
    isPuzzleActive = true;
    puzzleRoot.SetActive(true);

    // 👇 AÑADE ESTO: Organizar las piezas en grilla
    ArrangePiecesInGrid();

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

    public void ClosePuzzle()
    {
        if (!isPuzzleActive) return;
        puzzleRoot.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPuzzleActive = false;
    }

    public bool IsPuzzleActive => isPuzzleActive;

    // Llama a este método desde un botón o después de cada rotación (opcional)
    public void CheckSolution()
    {
        var pieces = puzzleRoot.GetComponentsInChildren<PuzzlePiece3D>();
        if (pieces.All(p => p.IsCorrect()))
        {
            Debug.Log("¡Puzzle resuelto!");
            isDoorRaised = true;
            ClosePuzzle();
            StartCoroutine(RaiseDoor());
        }
    }

    System.Collections.IEnumerator RaiseDoor()
    {
        Vector3 start = doorTransform.position;
        Vector3 target = start + Vector3.up * doorRaiseAmount;
        float duration = doorRaiseAmount / doorRaiseSpeed;
        float elapsed = 0;
        while (elapsed < duration)
        {
            doorTransform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        doorTransform.position = target;
    }

// Dentro de PuzzleManager3D.cs
public void ArrangePiecesInGrid()
{
    if (puzzleRoot == null) return;

    // Obtener todos los hijos con SpriteRenderer
    Transform[] children = puzzleRoot.GetComponentsInChildren<Transform>();
    var piecesList = new System.Collections.Generic.List<Transform>();

    foreach (Transform child in children)
    {
        if (child == puzzleRoot.transform) continue;
        if (child.GetComponent<SpriteRenderer>() != null)
        {
            piecesList.Add(child);
        }
    }

    if (piecesList.Count != 9)
    {
        Debug.LogWarning($"Se esperaban 9 piezas, pero se encontraron {piecesList.Count}.");
        return;
    }

    // Ordenar por nombre numérico (1 SP, 2 SP, ..., 9 SP)
    piecesList.Sort((a, b) =>
    {
        string nameA = a.name.Replace(" SP", "").Replace("SP", "").Trim();
        string nameB = b.name.Replace(" SP", "").Replace("SP", "").Trim();
        int numA = int.TryParse(nameA, out int nA) ? nA : 999;
        int numB = int.TryParse(nameB, out int nB) ? nB : 999;
        return numA.CompareTo(numB);
    });

    // Organizar en grilla 3x3
    float spacingX = 1.2f;
    float spacingY = 1.2f;
    int index = 0;

    for (int row = 0; row < 3; row++)
    {
        for (int col = 0; col < 3; col++)
        {
            if (index >= piecesList.Count) break;
            Transform piece = piecesList[index];
            piece.SetParent(puzzleRoot.transform, true); // mantener posición local

            float x = (col - 1) * spacingX; // centrado: -1, 0, +1
            float y = (1 - row) * spacingY; // fila 0 = arriba
            piece.localPosition = new Vector3(x, y, 0);

            index++;
        }
    }
}

}