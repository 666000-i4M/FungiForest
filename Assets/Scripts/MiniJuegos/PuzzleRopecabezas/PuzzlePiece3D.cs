// PuzzlePiece3D.cs
using UnityEngine;

public class PuzzlePiece3D : MonoBehaviour
{
    public int correctRotation; // 0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°
    private int currentRotation = 0;

    private void OnMouseDown()
    {
        if (!PuzzleManager3D.Instance?.IsPuzzleActive ?? false) return;

        currentRotation = (currentRotation + 1) % 4;
        transform.Rotate(0, 0, -90f);
    }

    public bool IsCorrect() => currentRotation == correctRotation;
}