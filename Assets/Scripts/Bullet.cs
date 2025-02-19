using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction;

    private void FixedUpdate()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Hit(200);
            Destroy(gameObject); // Destruye la bala si golpea un enemigo
            return;
        }

        //  Verifica si choca con una pared o puerta
        if (collider.CompareTag("Wall") || collider.CompareTag("Door"))
        {
            Destroy(gameObject); // Destruye la bala si toca una pared o puerta
        }
    }
}
