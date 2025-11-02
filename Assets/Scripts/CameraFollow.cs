using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target;
    public PlayerInput playerInput;
 
    [Header("Configuración de cámara")]
    [SerializeField] Vector3 offset = new Vector3(0, 10f, -5f);
    [SerializeField] float sensitivity = 120f;
    [SerializeField] float smoothSpeed = 10f;
    [SerializeField] float minPitch = -20f;
    [SerializeField] float maxPitch = 60f;
    [SerializeField] float resetSpeed = 2f; 

    float yaw;
    float pitch;
    Vector2 lookInput;
 
    [Header("Diálogo")]
    public GameObject dialogueCanvas;
 
    void Start()
    {
        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();
 
        if (dialogueCanvas == null)
            dialogueCanvas = GameObject.Find("DialogueCanvas");
 
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
 
        Cursor.lockState = CursorLockMode.Locked;
    }
 
    void LateUpdate()
    {
        if (target == null) return;
 
        if (dialogueCanvas != null && dialogueCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }
 
        RotateCamera();
        FollowTarget();
    }
 
    void RotateCamera()
    {
        // Leer input de rotación
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
 
        // Actualizar rotación
        if (lookInput.sqrMagnitude > 0.01f)
        {
            yaw += lookInput.x * sensitivity * Time.deltaTime;
            pitch -= lookInput.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else // 👈 NUEVO: Sin input, reset a espaldas
        {
            float targetYaw = target.eulerAngles.y;
            yaw = Mathf.LerpAngle(yaw, targetYaw, resetSpeed * Time.deltaTime);
        }
    }

    void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
 
        // Calcular posición deseada
        Vector3 desiredPosition = target.position + rotation * offset;
 
        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
 
        // Mirar al jugador
        transform.LookAt(target.position + Vector3.up * 1.8f);
    }
 
    public Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
 
    public Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0;
        return right.normalized;
    }
}