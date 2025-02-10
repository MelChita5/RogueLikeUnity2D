using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovimiento : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint; // Punto de disparo
    public Vector2 moveDir;
    public float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;



    private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Inicializaci�n del Rigidbody2D
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement(); // Maneja las entradas para el movimiento y disparo

        if (Input.GetKeyDown(KeyCode.Space)) // Presionar Espacio para disparar
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Move(); // Aplica el movimiento usando FixedUpdate
    }

    void InputManagement()
    {
        float moveX = UnityEngine.Input.GetAxisRaw("Horizontal");
        float moveY = UnityEngine.Input.GetAxisRaw("Vertical");

        // Asigna la direcci�n de movimiento y controla las animaciones de acuerdo a las teclas presionadas
        moveDir = new Vector2(moveX, moveY).normalized; // Normalizamos para evitar velocidad diagonal m�s r�pida

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

    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned!");
            return;
        }

        // Crear la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Asegurarnos de que la bala tiene el componente Bullet
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript == null)
        {
            Debug.LogError("The Bullet prefab does not have the Bullet script attached!");
            return;
        }

        // Definir la direcci�n de la bala seg�n la �ltima direcci�n del personaje
        if (_animator.GetBool("lookingRight"))
            bulletScript.direction = Vector2.right;
        else if (_animator.GetBool("lookingLeft"))
            bulletScript.direction = Vector2.left;
        else if (_animator.GetBool("lookingUp"))
            bulletScript.direction = Vector2.up;
        else if (_animator.GetBool("lookingDown"))
            bulletScript.direction = Vector2.down;
    }



    void Move()
    {
        // Aplica la velocidad al Rigidbody2D para mover al jugador
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
