using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importar SceneManager

public class PlayerMovimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDir;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Vida")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        moveDir = Vector2.zero;
        rb.velocity = Vector2.zero;

        _animator.SetBool("isMoving", false);
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(1);
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        bool isMoving = moveDir.sqrMagnitude > 0;
        _animator.SetBool("isMoving", isMoving);

        _animator.SetBool("lookingRight", moveX > 0);
        _animator.SetBool("lookingLeft", moveX < 0);
        _animator.SetBool("lookingUp", moveY > 0);
        _animator.SetBool("lookingDown", moveY < 0);
    }

    void Move()
    {
        if (isDead) return;
        rb.velocity = moveDir * moveSpeed;
    }

    void Shoot()
    {
        if (isDead) return;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("BulletPrefab o FirePoint no están asignados.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript == null)
        {
            Debug.LogError("El prefab de la bala no tiene el script Bullet adjunto.");
            return;
        }

        if (_animator.GetBool("lookingRight")) bulletScript.direction = Vector2.right;
        else if (_animator.GetBool("lookingLeft")) bulletScript.direction = Vector2.left;
        else if (_animator.GetBool("lookingUp")) bulletScript.direction = Vector2.up;
        else if (_animator.GetBool("lookingDown")) bulletScript.direction = Vector2.down;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        Debug.Log("Recibiendo daño... Salud actual: " + currentHealth);

        currentHealth -= damage;
        _animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Debug.Log("Jugador ha llegado a 0 de salud, muriendo...");
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;  // Asegurarnos de que la función se ejecute solo una vez

        isDead = true;  // Marcar como muerto
        _animator.SetTrigger("Die");  // Activar la animación de muerte
        rb.velocity = Vector2.zero;  // Detener el movimiento
        GetComponent<Collider2D>().enabled = false;  // Desactivar el collider para evitar colisiones
        rb.simulated = false;  // Desactivar la física del Rigidbody2D

        // Usar Invoke para llamar a RestartLevel después de un retraso, para dar tiempo a la animación de muerte
        float delay = 2.5f; // Ajusta este valor según la duración de tu animación de muerte
        Invoke("RestartLevel", delay);
    }

    public void RestartLevel()
    {
        // Aquí ya puedes reiniciar la escena una vez que la animación haya terminado
        Debug.Log("Reiniciando el nivel...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reiniciar el nivel actual
    }

}
