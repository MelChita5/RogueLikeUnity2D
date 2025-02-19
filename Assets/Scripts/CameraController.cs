using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed = 3f; // Velocidad de transición de la cámara
    private Transform targetRoom; // La habitación actual en la que está el jugador

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Llamado por el RoomManager cuando el jugador entra a una nueva habitación
    public void MoveToRoom(Transform newRoom)
    {
        if (targetRoom != newRoom)
        {
            targetRoom = newRoom;
            StopAllCoroutines();
            StartCoroutine(MoveCamera());
        }
    }

    IEnumerator MoveCamera()
    {
        while (Vector3.Distance(transform.position, targetRoom.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetRoom.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(targetRoom.position.x, targetRoom.position.y, transform.position.z);
    }
}
