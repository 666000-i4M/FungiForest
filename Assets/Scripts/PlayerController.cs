using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float walkSpeed = 3f;        // Velocidad normal (caminar)
    [SerializeField] float runSpeed = 10f;         // Velocidad correr (Shift)
    [SerializeField] float jumpHeight = 2f;       // Altura del salto
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float rotationSpeed = 10f;

    [Header("Referencias")]
    public CameraFollow cameraFollow;

    CharacterController controller;
    Animator animator;
    PlayerInput playerInput;

    Vector3 velocity;
    Vector2 input;
    bool isGrounded;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

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

        // Leer input del movimiento
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        // Leer input de correr (Shift)
        bool isRunning = playerInput.actions["Run"].IsPressed();

        // Obtener dirección relativa a la cámara
        Vector3 camForward = cameraFollow.GetCameraForward();
        Vector3 camRight = cameraFollow.GetCameraRight();
        Vector3 move = camForward * input.y + camRight * input.x;

        // Velocidad según si corre o camina
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Mover al jugador
        if (move.magnitude > 0.01f)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        bool isMoving = move.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving && !isRunning);  
        animator.SetBool("isRunning", isMoving && isRunning);  
        animator.SetBool("Grounded", isGrounded);
    }

    void HandleGravity()
    {
        isGrounded = controller.isGrounded;

    // SALTO INSTANTÁNEO: Trigger + Física
    if (isGrounded && playerInput.actions["Jump"].WasPressedThisFrame())
    {
        animator.SetTrigger("Jump");  // ← INSTANTÁNEO, sincroniza con física
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    if (isGrounded && velocity.y < 0)
        velocity.y = -2f;

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
    }
}