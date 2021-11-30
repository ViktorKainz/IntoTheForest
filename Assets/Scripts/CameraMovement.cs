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
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
        
        Quaternion r = transform.rotation;
        r.x = 0;
        r.z = 0;
        Vector3 v;
        
        if (Input.GetMouseButton(0))
        {
            v = new Vector3(-Input.GetAxis("Mouse X") * dragSpeed * scale, 0, -Input.GetAxis("Mouse Y") * dragSpeed * scale);
            pos += r * v;
            //pos += transform.forward * Input.GetAxis("Mouse X") * dragSpeed * scale;
            //pos.x -= Input.GetAxis("Mouse X") * dragSpeed * scale;
            //pos.z -= Input.GetAxis("Mouse Y") * dragSpeed * scale;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            v = new Vector3(-fov, 0, 0);
            pos += r * v;
        }
        if (Input.GetKey(KeyCode.D))
        {
            v = new Vector3(fov, 0, 0);
            pos += r * v;
        }
        if (Input.GetKey(KeyCode.S))
        {
            v = new Vector3(0, 0, -fov);
            pos += r * v;
        }
        if (Input.GetKey(KeyCode.W))
        {
            v = new Vector3(0, 0, fov);
            pos += r * v;
        }   
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 ea = transform.eulerAngles;
            transform.eulerAngles = new Vector3(ea.x, ea.y - 5, ea.z);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 ea = transform.eulerAngles;
            transform.eulerAngles = new Vector3(ea.x, ea.y + 5, ea.z);
        }
        
        transform.position = pos;
    }
}