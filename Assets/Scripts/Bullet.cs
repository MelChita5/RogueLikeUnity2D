using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction; // La dirección en la que viajará la bala

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Si toca a un enemigo
        {
            Destroy(gameObject); // Destruir la bala
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); // Destruye la bala si sale de la pantalla
    }
}
