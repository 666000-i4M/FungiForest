using UnityEngine;

public class AlwaysVisibleCursor : MonoBehaviour
{
    void Awake()
    {
        // Inicialmente visible y desbloqueado
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Forzamos cada frame que el cursor esté visible y desbloqueado
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
