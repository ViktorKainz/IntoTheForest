using System;

using UnityEngine;

public class TerrainField : MonoBehaviour
{
    public String type;
    public LevelGeneration level;
    public GameObject figure;
    public int x;
    public int y;
    
    private Color startcolor;

    // Start is called before the first frame update
    void Start()
    {
        if (figure != null)
        {
            figure = Instantiate(figure, new Vector3(x * level.size.x, 0, y * level.size.z), Quaternion.Euler(0, 0 , 0));
            figure.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log(type + " x: " + x + " y: " + y);
        var renderer = GetComponent<Renderer>();
        startcolor = renderer.material.color;
        renderer.material.color = Color.yellow;
        Debug.Log(renderer.material.color); 
    }
}
