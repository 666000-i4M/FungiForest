using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float speed = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float rotationSpeed = 10f;

    [Header("Referencias")]
    public CameraFollow cameraFollow;

    CharacterController controller;
    Animator animator;
    PlayerInput playerInput;

    Vector3 velocity;      // Para gravedad
    Vector2 input;         // Input del jugador

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        // Si no hay cámara asignada, buscar automáticamente la principal
        if (cameraFollow == null)
        {
            CameraFollow cam = FindObjectOfType<CameraFollow>();
            if (cam != null)
                cameraFollow = cam;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
    }

    void HandleMovement()
    {
        if (playerInput == null || cameraFollow == null)
            return;

        // Leer input del jugador
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        // Obtener dirección relativa a la cámara
        Vector3 camForward = cameraFollow.GetCameraForward();
        Vector3 camRight = cameraFollow.GetCameraRight();
        Vector3 move = camForward * input.y + camRight * input.x;

        // Mover al jugador
        if (move.magnitude > 0.01f)
        {
            controller.Move(move * speed * Time.deltaTime);

            // Rotar suavemente hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Animaciones
        bool isMoving = move.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
    }

    void HandleGravity()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Mantener contacto con el suelo

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
