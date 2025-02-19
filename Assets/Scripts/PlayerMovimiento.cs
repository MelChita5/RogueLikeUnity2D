using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    private Animator _animator;
    private SpriteRenderer spriteRenderer;
    private int deathKeyPressCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        moveDir = Vector2.zero;
        rb.velocity = Vector2.zero;
        _animator.SetBool("isMoving", false);
        healthText.text = currentHealth.ToString();
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        Move();  // Movemos en Update() para una mejor respuesta

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        if (Input.GetKeyDown(KeyCode.M))
        {
            deathKeyPressCount++;
            Debug.Log("Tecla M presionada: " + deathKeyPressCount + " veces.");
            if (deathKeyPressCount >= 5)
                Die();
        }
    }

    void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        // Aplicamos las animaciones inmediatamente para reflejar el input sin retrasos
        _animator.SetBool("isMoving", moveDir.sqrMagnitude > 0);

        if (moveX > 0)  // Mirando a la derecha
        {
            _animator.SetBool("lookingRight", true);
            _animator.SetBool("lookingLeft", false);
            _animator.SetBool("lookingUp", false);
            _animator.SetBool("lookingDown", false);
        }
        else if (moveX < 0)  // Mirando a la izquierda
        {
            _animator.SetBool("lookingRight", false);
            _animator.SetBool("lookingLeft", true);
            _animator.SetBool("lookingUp", false);
            _animator.SetBool("lookingDown", false);
        }
        else if (moveY > 0)  // Mirando arriba
        {
            _animator.SetBool("lookingRight", false);
            _animator.SetBool("lookingLeft", false);
            _animator.SetBool("lookingUp", true);
            _animator.SetBool("lookingDown", false);
        }
        else if (moveY < 0)  // Mirando abajo
        {
            _animator.SetBool("lookingRight", false);
            _animator.SetBool("lookingLeft", false);
            _animator.SetBool("lookingUp", false);
            _animator.SetBool("lookingDown", true);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
            TakeDamage(20);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthText.text = Mathf.Clamp(currentHealth, 0, maxHealth).ToString();
        Debug.Log("Recibiendo daño... Salud actual: " + currentHealth);

        _animator.SetTrigger("Hurt");
        StartCoroutine(BlinkEffect());

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        _animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;

        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2.5f);
        RestartLevel();
        GoToMenuInicial();
    }

    public void GoToMenuInicial()
    {
        Debug.Log("Regresando al menú inicial...");
        SceneManager.LoadScene("MenuInicial");
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
