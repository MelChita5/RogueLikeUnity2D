using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovimiento : MonoBehaviour { 

public float moveSpeed;
Rigidbody2D rb;
[HideInInspector]
public float lastHorizontalVector;
[HideInInspector]
public float lastVerticalVector;
[HideInInspector]
public Vector2 moveDir;

    private Animator _animator;

void Start()
{
    rb = GetComponent<Rigidbody2D>();  // Inicialización del Rigidbody2D
        _animator = GetComponent<Animator>();
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
        float moveX = UnityEngine.Input.GetAxisRaw("Horizontal");
        float moveY = UnityEngine.Input.GetAxisRaw("Vertical");

        // Asigna la dirección de movimiento
        moveDir = new Vector2(moveX, moveY).normalized;

        // Detectar si se está moviendo
        bool isMoving = moveDir.sqrMagnitude > 0;

        // Aplicar a la animación el estado de movimiento
        _animator.SetBool("isMoving", isMoving);

        // Determinar la dirección en la que mira el jugador
        if (moveX > 0)
        {
            _animator.SetBool("lookingRight", true);
            _animator.SetBool("lookingLeft", false);
            _animator.SetBool("lookingUp", false);
            _animator.SetBool("lookingDown", false);
        }
        else if (moveX < 0)
        {
            _animator.SetBool("lookingRight", false);
            _animator.SetBool("lookingLeft", true);
            _animator.SetBool("lookingUp", false);
            _animator.SetBool("lookingDown", false);
        }
        else if (moveY > 0)
        {
            _animator.SetBool("lookingRight", false);
            _animator.SetBool("lookingLeft", false);
            _animator.SetBool("lookingUp", true);
            _animator.SetBool("lookingDown", false);
        }
        else if (moveY < 0)
        {
            _animator.SetBool("lookingRight", false);
            _animator.SetBool("lookingLeft", false);
            _animator.SetBool("lookingUp", false);
            _animator.SetBool("lookingDown", true);
        }
    }
    void Move()
{
    // Aplica la velocidad al Rigidbody2D
    rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
}
}