using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCameraToRoom : MonoBehaviour
{
    public Transform camPosition;
    public Transform mainCamera;

    private void Start()
    {
        mainCamera = FindFirstObjectByType<Camera>().transform;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mainCamera.position = camPosition.position;
        }
    }
}
