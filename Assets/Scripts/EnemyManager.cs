using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    //[SerializeField] float timeBetweenSpawns = 0.5f;
    //float currentTimeBetweenSpawns;
    Transform enemiesParent;

    public static EnemyManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        enemiesParent = GameObject.Find("Enemies").transform;
    }
    
    public void SpawnEnemiesInRoom(Vector3 roomPosition)
    {
        int enemyCount = Random.Range(1, 2); // Asegura que al menos haya 1 enemigo
        EnemyCounter.Instance.SetSpawningState(true); //  Indica que estamos spawneando

        for (int i = 0; i < enemyCount; i++)
        {
            //Vector3 spawnPos = roomPosition + (Vector3)RandomPosition();
            Vector3 spawnPos = RandomPosition(roomPosition);

            var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.transform.SetParent(enemiesParent);

            EnemyCounter.Instance.UpdateEnemyCount(1);
        }

        EnemyCounter.Instance.SetSpawningState(false); //  Indica que terminamos de spawnear
    }



    Vector2 RandomPosition(Vector3 roomPosition)
    {
        // Asegúrate de que roomWidth y roomHeight estén ajustados al tamaño de tu sala.
        float roomWidth = 14f;  // Ajusta al tamaño real de tu sala
        float roomHeight = 8f;

        // Ajusta estos valores para reducir el tamaño del área de spawn
        float spawnPadding = 2f;  // Esto crea un margen de seguridad dentro de la habitación

        // Calcula una posición aleatoria dentro de los límites reducidos.
        float randomX = Random.Range(-roomWidth / 2 + spawnPadding, roomWidth / 2 - spawnPadding);
        float randomY = Random.Range(-roomHeight / 2 + spawnPadding, roomHeight / 2 - spawnPadding);

        // Retorna la posición ajustada en el espacio de la habitación
        return new Vector2(roomPosition.x + randomX, roomPosition.y + randomY);
    }

   

    void SpawnEnemy(Vector3 roomPosition) // Agregamos el parámetro
    {
        var e = Instantiate(enemyPrefab, RandomPosition(roomPosition), Quaternion.identity);
        e.transform.SetParent(enemiesParent);
    }


    public void DestroyAllEnemies()
    {
        foreach (Transform e in enemiesParent)
        {
            Destroy(e.gameObject);
        }
    }
}