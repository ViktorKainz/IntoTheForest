using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dragSpeed = 8f;
    
    private float scale;
    
    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    void Update()
    {
        Vector3 pos = transform.position;
 
        scale = Camera.main.orthographicSize;
 
        if (Input.GetMouseButton(0))
        {
            pos.x -= Input.GetAxis("Mouse X") * dragSpeed * scale;
            pos.z -= Input.GetAxis("Mouse Y") * dragSpeed * scale;
        }
 
        transform.position = pos;
        
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}