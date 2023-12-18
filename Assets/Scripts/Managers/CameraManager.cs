using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of camera movement
    public float zoomSpeed = 10f; // Speed of zoom
    public float minZoom = 5f; // Minimum zoom level
    public float maxZoom = 50f; // Maximum zoom level

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Get the main camera
    }

    void Update()
    {
        // Move the camera with WASD keys
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, moveY, 0);

        // Zoom the camera with the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newSize = cam.orthographicSize - scroll;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }
}
