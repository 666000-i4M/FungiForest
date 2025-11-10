using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; //velocidad
    public float jumpForce = 5f; //fuerza salto
    public float sprintMultiplier = 1.5f; //mult para sprint
    private Rigidbody rb;
    private bool isGrounded; //verificacion si el personaje esta en contacto con el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //movimiento con teclas WASD
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D
        float moveVertical = Input.GetAxis("Vertical"); // W/S
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized * speed;

        //sprint con tecla Shift
        if (Input.GetKey(KeyCode.LeftShift))
            movement *= sprintMultiplier;

        //aplicar movimiento
        rb.MovePosition(transform.position + movement * Time.deltaTime);

        //salto con tecla Space
        if (Input.GetKeyDown(KeyCode.Q) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    //para detectar si esta en el suelo
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
