using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   [Header("Movimiento")]
    [SerializeField] float speed = 5f;
    [SerializeField] float gravity = -9.81f;
    //[SerializeField] float jumpHeight = 1.5f;
 
    CharacterController controller;
    PlayerInput playerInput;
    Vector3 velocity;
    bool isGrounded;
    Vector2 input;
    bool isMoving;
 
    [SerializeField] float rotationSpeed = 10f;
 
    Animator animator;
 
    [SerializeField] Transform cameraTransform; 
    [SerializeField] CameraFollow cameraFollow; 

    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask interactLayer = 1; // Default layer
 
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }
 
    void Update()
    {
        MovePlayer();

        //ACCIONES APARTE
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
            {
                if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
 
    void MovePlayer()
    {
        // Leer input
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 camForward = cameraFollow.GetCameraForward();
        Vector3 camRight = cameraFollow.GetCameraRight();
 
        Vector3 move = (camForward * input.y + camRight * input.x).normalized;
 
        // Mover al personaje
        controller.Move(move * speed * Time.deltaTime);
 
        // Rotar hacia la dirección de movimiento
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
 
        // Animación
        bool isMoving = move.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
 
        // Gravedad
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
 
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    

}
