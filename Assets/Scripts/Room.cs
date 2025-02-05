using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int RoomIndex { get; set; }

    [SerializeField] private GameObject doorUp;
    [SerializeField] private GameObject doorDown;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;

    [SerializeField] private BoxCollider2D wallUp;
    [SerializeField] private BoxCollider2D wallDown;
    [SerializeField] private BoxCollider2D wallLeft;
    [SerializeField] private BoxCollider2D wallRight;

    
    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            doorUp.SetActive(true);
            wallUp.enabled = false; // Desactivar collider de la pared superior
        }
        else if (direction == Vector2Int.down)
        {
            doorDown.SetActive(true);
            wallDown.enabled = false; // Desactivar collider de la pared inferior
        }
        else if (direction == Vector2Int.left)
        {
            doorLeft.SetActive(true);
            wallLeft.enabled = false; // Desactivar collider de la pared izquierda
        }
        else if (direction == Vector2Int.right)
        {
            doorRight.SetActive(true);
            wallRight.enabled = false; // Desactivar collider de la pared derecha
        }
    }
    



}

