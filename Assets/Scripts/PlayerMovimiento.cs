using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    private Animator _animator;
    private SpriteRenderer spriteRenderer;

    private int deathKeyPressCount = 0; // Contador de veces que se presiona la tecla

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // Contador de tecla presionada (M)
        if (Input.GetKeyDown(KeyCode.M))
        {
            deathKeyPressCount++;
            Debug.Log("Tecla M presionada: " + deathKeyPressCount + " veces.");

            if (deathKeyPressCount >= 5)
            {
                Debug.Log("Muerte activada por la tecla.");
                Die();
            }
        }
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

        currentHealth -= damage;
        Debug.Log("Recibiendo daño... Salud actual: " + currentHealth);

        _animator.SetTrigger("Hurt");

        StartCoroutine(BlinkEffect());

        if (currentHealth <= 0)
        {
            Debug.Log("Jugador ha muerto.");
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        _animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;

        float delay = 2.5f;
        Invoke("RestartLevel", delay);
    }

    public void RestartLevel()
    {
        Debug.Log("Reiniciando el nivel...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator BlinkEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
