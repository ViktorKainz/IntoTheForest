using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dragSpeed = 8f;
    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;
    public Bounds bounds;

    public void Initialize(int levelSize, float componentSize)
    {
        var center = new Vector3(levelSize / 2f * componentSize, 900, levelSize / 2f * componentSize);
        transform.position = center;
        transform.rotation = Quaternion.Euler (60, 0, 0);
        center.y = 0;
        bounds = new Bounds(center, center * 2.5f);
    }

    void LateUpdate()
    {
        var t = transform;
        var pos = t.position;
        var angles = t.eulerAngles;
        var rotation = t.rotation;
        rotation.x = 0;
        rotation.z = 0;
        var c = Camera.main;
        var scale = c.orthographicSize;
        var fov = c.fieldOfView;
        
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);

        if (Input.GetMouseButton(0))
        {
            pos += rotation * new Vector3(-Input.GetAxis("Mouse X") * dragSpeed * scale, 0,
                -Input.GetAxis("Mouse Y") * dragSpeed * scale);
        }
        if (Input.GetMouseButton(1))
        {
            angles.y += Input.GetAxis("Mouse X") * dragSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos += rotation * new Vector3(-fov * Time.deltaTime * dragSpeed * 2, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += rotation * new Vector3(fov * Time.deltaTime * dragSpeed * 2, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos += rotation * new Vector3(0, 0, -fov * Time.deltaTime * dragSpeed * 2);
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos += rotation * new Vector3(0, 0, fov * Time.deltaTime * dragSpeed * 2);
        }   
        if (Input.GetKey(KeyCode.Q))
        {
            angles.y -= fov/50;
        }
        if (Input.GetKey(KeyCode.E))
        {
            angles.y += fov/50;
        }
        if (Input.GetKey(KeyCode.C))
        {
            angles.y = 0;
            pos = bounds.center;
            pos.y = 900;
            fov = 60;
        }
        if (bounds.Contains(pos+new Vector3(0,-pos.y,0)))
        {
            transform.position = pos;
        }
        transform.eulerAngles = angles;
        Camera.main.fieldOfView = fov;
    }
}