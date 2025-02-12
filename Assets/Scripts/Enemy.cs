using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] float speed = 2f;

    private int currentHealth;

    Animator _anim;
    Transform target;

    private void Start()
    {
        currentHealth = maxHealth;

        // Intentamos encontrar al jugador
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró el objeto 'Player' en la escena.");
        }

        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target == null) return; // Si el jugador no existe, no hacemos nada

        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;
    }
}