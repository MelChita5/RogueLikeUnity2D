using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] float speed = 2f;

    private int currentHealth;
    private Animator _anim;
    private Transform target;

    private void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.Find("Player").transform;
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;

            //Voltear al enemigo para que mire al jugador
            FlipSprite(direction.x);
        }
    }

    private void FlipSprite(float directionX)
    {
        if (directionX > 0)
            transform.localScale = new Vector3(1, 1, 1);  // Mirando a la derecha
        else if (directionX < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Mirando a la izquierda
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        _anim.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            EnemyCounter.Instance.UpdateEnemyCount(-1);
            EnemyCounter.Instance.CheckIfEnemiesRemain(); 
        }
    }

}
