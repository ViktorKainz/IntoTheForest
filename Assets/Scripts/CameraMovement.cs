using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mainSpeed = 100.0f;
    public float camSens = 0.25f;
    public float minHeight = 200;
    public float maxHeight = 5000;
    private Vector3 lastMouse = Vector3.zero;

    void Update()
    {
        var t = transform;
        if (Input.GetMouseButton(1))
        {
            if (lastMouse != Vector3.zero)
            {
                lastMouse = Input.mousePosition - lastMouse;
                lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
                lastMouse = new Vector3(t.eulerAngles.x + lastMouse.x, t.eulerAngles.y + lastMouse.y, 0);
                t.eulerAngles = lastMouse;
            }
            lastMouse = Input.mousePosition;
        }
        else
        {
            lastMouse = Vector3.zero;
        }
        
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        {
            p = p * mainSpeed * Time.deltaTime;
            t.Translate(p);
        }

        if (t.position.y < minHeight)
        {
            var newPosition = t.position;
            newPosition.y = minHeight;
            t.position = newPosition;
        }
        
        if (t.position.y > maxHeight)
        {
            var newPosition = t.position;
            newPosition.y = maxHeight;
            t.position = newPosition;
        }
    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }

        return p_Velocity;
    }
}