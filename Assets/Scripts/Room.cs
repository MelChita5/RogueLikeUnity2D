using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps; 

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    public Vector2Int RoomIndex { get; set; }

    private void Start()
    {
        // Inicializa las puertas como transparentes al inicio del juego
        SetDoorTransparency(topDoor, true);
        SetDoorTransparency(bottomDoor, true);
        SetDoorTransparency(leftDoor, true);
        SetDoorTransparency(rightDoor, true);
    }

    public void CloseDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            SetDoorTransparency(topDoor, true); // Hace la puerta transparente
            topDoor.GetComponent<BoxCollider2D>().enabled = true; // Mantiene la colisión activa
        }
        if (direction == Vector2Int.down)
        {
            SetDoorTransparency(bottomDoor, true);
            bottomDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (direction == Vector2Int.left)
        {
            SetDoorTransparency(leftDoor, true);
            leftDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (direction == Vector2Int.right)
        {
            SetDoorTransparency(rightDoor, true);
            rightDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            SetDoorTransparency(topDoor, false); // Restaura el color original de la puerta
            topDoor.GetComponent<BoxCollider2D>().enabled = false; // Desactiva la colisión
        }
        if (direction == Vector2Int.down)
        {
            SetDoorTransparency(bottomDoor, false);
            bottomDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.left)
        {
            SetDoorTransparency(leftDoor, false);
            leftDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.right)
        {
            SetDoorTransparency(rightDoor, false);
            rightDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void SetDoorTransparency(GameObject door, bool isTransparent)
    {
        Tilemap tilemap = door.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Color color = tilemap.color;
            color.a = isTransparent ? 0f : 1f; // Cambia la transparencia (0 = transparente, 1 = opaco)
            tilemap.color = color;
        }
    }
}