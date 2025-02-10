using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
  
    void Start()
    {
        Invoke("WaitToEnd", 10);
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuInicial");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}
