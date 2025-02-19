using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance { get; private set; }
    private int totalEnemies = 0;
    private bool isSpawning = true; //  Evita que se revise antes de tiempo

    [SerializeField] private TextMeshProUGUI enemyCounterText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateEnemyCount(0);
    }

    
    public void SetSpawningState(bool state)
    {
        isSpawning = state;
        if (!isSpawning) CheckIfEnemiesRemain();
    }

    public void CheckIfEnemiesRemain()
    {
        if (totalEnemies <= 0 && !isSpawning) 
        {
            LoadMenuScene();
        }
    }

    public void UpdateEnemyCount(int change)
    {
        totalEnemies += change;
        enemyCounterText.text = "Enemies: " + Mathf.Max(totalEnemies, 0);
    }

    private void LoadMenuScene()
    {
        Debug.Log("No quedan enemigos. Cargando escena MenuInicial...");
        SceneManager.LoadScene("MenuInicial");
    }
}

