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

    private GameObject GetDoorByDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return topDoor;
        if (direction == Vector2Int.down) return bottomDoor;
        if (direction == Vector2Int.left) return leftDoor;
        if (direction == Vector2Int.right) return rightDoor;
        return null;
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


/*
private void SetDoorState(GameObject door, bool isActive)
{
    if (door == null) return;

    // Activar/desactivar el TilemapRenderer para mostrar/ocultar la puerta
    TilemapRenderer tilemapRenderer = door.GetComponent<TilemapRenderer>();
    if (tilemapRenderer != null)
    {
        tilemapRenderer.enabled = isActive; // Si isActive es true, se muestra, si es false, se oculta
    }

    // Activar/desactivar BoxCollider2D
    BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
    if (collider != null)
    {
        collider.enabled = !isActive; // Si isActive es true, el collider se desactiva, si es false, se activa
        Debug.Log($"Puerta {door.name} -> Activa: {isActive}, Collider Activo: {collider.enabled}");
        Debug.Log($"Puerta {door.name} - TilemapRenderer: {tilemapRenderer.enabled}, BoxCollider2D: {collider.enabled}");

    }
}
*/


/*
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

    private GameObject GetDoorByDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return topDoor;
        if (direction == Vector2Int.down) return bottomDoor;
        if (direction == Vector2Int.left) return leftDoor;
        if (direction == Vector2Int.right) return rightDoor;
        return null;
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


public class Room : MonoBehaviour
{
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    public Vector2Int RoomIndex { get; set; }

    private void Start()
    {
        // Asegurar que las puertas empiecen cerradas (ocultas con colisión activada)
        SetDoorState(topDoor, false);
        SetDoorState(bottomDoor, false);
        SetDoorState(leftDoor, false);
        SetDoorState(rightDoor, false);
    }

    public void CloseDoor(Vector2Int direction)
    {
        GameObject door = GetDoorByDirection(direction);
        if (door != null)
            SetDoorState(door, false); // Se desactiva la puerta (sprite invisible y colisión activa)
    }

    public void OpenDoor(Vector2Int direction)
    {
        GameObject door = GetDoorByDirection(direction);
        if (door != null)
            SetDoorState(door, true); // Se activa la puerta (sprite visible y colisión desactivada)
    }

    private GameObject GetDoorByDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return topDoor;
        if (direction == Vector2Int.down) return bottomDoor;
        if (direction == Vector2Int.left) return leftDoor;
        if (direction == Vector2Int.right) return rightDoor;
        return null;
    }

    private void SetDoorState(GameObject door, bool isActive)
    {
        if (door == null) return;

        // Activar/desactivar el TilemapRenderer para mostrar/ocultar la puerta
        TilemapRenderer tilemapRenderer = door.GetComponent<TilemapRenderer>();
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = isActive;
        }

        // Activar/desactivar colisión correctamente
        BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = !isActive; // Cuando la puerta está activa, el collider debe desactivarse
            Debug.Log($"Puerta en {door.name} -> Activa: {isActive}, Collider Activo: {collider.enabled}");
        }
    }

}
*/