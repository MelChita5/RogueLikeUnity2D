using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Inicializaci�n del Rigidbody2D
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        // Obt�n los valores de entrada sin procesarlos a�n
        float moveX = UnityEngine.Input.GetAxisRaw("Horizontal");
        float moveY = UnityEngine.Input.GetAxisRaw("Vertical");

        // Asigna la direcci�n de movimiento con la normalizaci�n para evitar velocidades diagonales m�s r�pidas
        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
        }


    }

    void Move()
    {
        // Aplica la velocidad al Rigidbody2D
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
