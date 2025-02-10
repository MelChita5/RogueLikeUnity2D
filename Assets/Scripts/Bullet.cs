using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction; // La dirección en la que viajará la bala

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
            Destroy(gameObject); // Destruye la bala si sale de la pantalla
        }       
    }
}
