using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    [Header("Referencias")]
    public Transform target;
    public PlayerInput playerInput;

    [Header("Configuración de cámara")]
    [SerializeField] Vector3 offset = new Vector3(0, 20f, -10f);
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

    // Variables para centrar cámara en NPC
    private Transform temporaryTarget = null;
    [SerializeField] private float tempCenterSpeed = 5f;

    void Awake()
    {
        Instance = this;
    }

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
        Cursor.visible = false;
    }

   void LateUpdate()
{
    if (target == null) return;

    if (dialogueCanvas != null && dialogueCanvas.activeSelf)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // ⚡ NO hacemos return, dejamos que la cámara siga actualizándose
    }
    else
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    if (temporaryTarget != null)
        CenterOnTemporaryTarget();
    else
    {
        RotateCamera();
        FollowTarget();
    }
}


    void RotateCamera()
    {
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();

        if (lookInput.sqrMagnitude > 0.01f)
        {
            yaw += lookInput.x * sensitivity * Time.deltaTime;
            pitch -= lookInput.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else
        {
            float targetYaw = target.eulerAngles.y;
            yaw = Mathf.LerpAngle(yaw, targetYaw, resetSpeed * Time.deltaTime);
        }
    }

    void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.8f);
    }

    private void CenterOnTemporaryTarget()
    {
        Vector3 lookTarget = temporaryTarget.position + Vector3.up * 1.8f;
        Vector3 direction = (lookTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, tempCenterSpeed * Time.deltaTime);

        // Mantener posición detrás del jugador
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

public void CenterCameraOnTarget(Transform newTarget)
{
    if (newTarget == null) return;

    // Calcula dirección hacia el NPC
    Vector3 direction = newTarget.position - transform.position;
    Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

    // Opcional: suavizado
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.5f);
}

    private void ResetTemporaryTarget()
    {
        temporaryTarget = null;
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
