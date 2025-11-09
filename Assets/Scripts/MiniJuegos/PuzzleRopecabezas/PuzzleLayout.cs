// PuzzleLayout.cs
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class PuzzleLayout : MonoBehaviour
{
    [Header("Configuración de grilla")]
    public Vector2 spacing = new Vector2(1.2f, 1.2f); // distancia entre piezas
    public bool autoFindChildren = true;
    public GameObject[] pieces; // si no usas autoFind, arrastra manualmente

    void Start()
    {
        ArrangePieces();
    }

    public void ArrangePieces()
    {
        GameObject[] targets = pieces;

        if (autoFindChildren)
        {
            // Busca todos los hijos directos que tengan SpriteRenderer
            var children = GetComponentsInChildren<Transform>();
            var list = new System.Collections.Generic.List<GameObject>();
            foreach (var child in children)
            {
                if (child == transform) continue; // omitir el propio PuzzleRot
                if (child.GetComponent<SpriteRenderer>() != null)
                {
                    list.Add(child.gameObject);
                }
            }
            targets = list.ToArray();
        }

        if (targets.Length != 9)
        {
            Debug.LogWarning($"PuzzleLayout: Se esperaban 9 piezas, pero se encontraron {targets.Length}.");
            return;
        }

        // Ordenar por nombre (asume que se llaman "1 SP", "2 SP", ..., "9 SP")
        System.Array.Sort(targets, (a, b) =>
        {
            string nameA = a.name.Replace(" SP", "").Replace("SP", "").Trim();
            string nameB = b.name.Replace(" SP", "").Replace("SP", "").Trim();
            int numA = int.TryParse(nameA, out int nA) ? nA : 999;
            int numB = int.TryParse(nameB, out int nB) ? nB : 999;
            return numA.CompareTo(numB);
        });

        // Organizar en grilla 3x3 (filas de izquierda a derecha, de arriba a abajo)
        int index = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (index >= targets.Length) break;
                Transform piece = targets[index].transform;
                piece.SetParent(transform, true); // mantener mundo o local según necesites

                // Posición: centro en (0,0), con offset
                float x = (col - 1) * spacing.x; // -1, 0, +1 → centrado
                float y = (1 - row) * spacing.y; // fila 0 = arriba (y positivo)
                piece.localPosition = new Vector3(x, y, 0);

                index++;
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Organizar Piezas Ahora")]
    void OrganizeInEditor()
    {
        ArrangePieces();
    }
#endif
}