using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    int roomWidth = 15;  // Ajusta el tamaño de las habitaciones aquí
    int roomHeight = 10;

    [SerializeField] int gridSizeX = 10;
    [SerializeField] int gridSizeY = 10;

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();


    private int[,] roomGrid;

    private int roomCount;

    private bool generationComplete = false;

    private void Start()
    {
        // Evita que gridSizeX o gridSizeY sean cero o negativos
        if (gridSizeX <= 0 || gridSizeY <= 0)
        {
            Debug.LogError("Error: gridSizeX y gridSizeY deben ser mayores a 0");
            return;
        }

        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }


    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
        }
        else if (roomCount < minRooms)
        {
            Debug.Log("RoomCount was less than the minimum amount of rooms. Trying again");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation complete, {roomCount} rooms created");
            generationComplete = true;
        }
    }


    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);

        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;

        roomObjects.Add(initialRoom);

        // Abrir puertas en todas las direcciones para la habitación inicial
        OpenDoors(initialRoom, x, y);
    }



    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY)
            return false;

        if (roomCount >= maxRooms)
            return false;

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
            return false;

        if (CountAdjacentRooms(roomIndex) > 1)
            return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;

        roomCount++;

        var newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;

        newRoom.name = $"Room-{roomCount}";
        roomObjects.Add(newRoom);

        OpenDoors(newRoom, x, y);

        // Spawnear enemigos en la sala recién creada
        EnemyManager.instance.SpawnEnemiesInRoom(GetPositionFromGridIndex(roomIndex));

        return true;
    }



    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);

    }


    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        // Neighbours
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        // Determine which doors to open based on the direction
        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            // Neighbouring room to the left
            newRoomScript.OpenDoor(Vector2Int.left);
            if (leftRoomScript != null)
                leftRoomScript.OpenDoor(Vector2Int.right);
        }

        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            // Neighbouring room to the right
            newRoomScript.OpenDoor(Vector2Int.right);
            if (rightRoomScript != null)
                rightRoomScript.OpenDoor(Vector2Int.left);
        }

        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            // Neighbouring room below
            newRoomScript.OpenDoor(Vector2Int.down);
            if (bottomRoomScript != null)
                bottomRoomScript.OpenDoor(Vector2Int.up);
        }

        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            // Neighbouring room above
            newRoomScript.OpenDoor(Vector2Int.up);
            if (topRoomScript != null)
                topRoomScript.OpenDoor(Vector2Int.down);
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }



    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && x < gridSizeX && roomGrid[x - 1, y] != 0) count++; // Vecino izquierdo
        if (x >= 0 && x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++; // Vecino derecho
        if (y > 0 && y < gridSizeY && roomGrid[x, y - 1] != 0) count++; // Vecino inferior
        if (y >= 0 && y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++; // Vecino superior

        return count;
    }



    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        // Reducir la distancia entre las habitaciones
        float newRoomWidth = 18f;  // Ajusta este valor
        float newRoomHeight = 10f; // Ajusta este valor

        return new Vector3(newRoomWidth * (gridX - gridSizeX / 2),
                           newRoomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
}
