using System;
using System.Linq;
using UnityEngine;

public class TerrainField : MonoBehaviour
{
    public String type;
    public LevelGeneration level;
    public GameObject figure;
    public int x;
    public int y;
    
    private Color[][] startColors;

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
        if (level.selected == this)
        {
            unselectField();
            level.selected = null;
        }
        else
        {
            if (level.selected != null)
            {
                level.selected.unselectField();
            }
            selectField();
            level.selected = this;
        }
    }

    public void selectField()
    {
        var renderer = GetComponent<Renderer>();
        var children = GetComponentsInChildren<Renderer>();
        startColors = new Color[children.Length+1][];
        startColors[0] = new Color[renderer.materials.Length];
        if (renderer.materials.Length > 1)
        {
            for (var i = 0; i < renderer.materials.Length; i++)
            {
                startColors[0][i] = renderer.materials[i].color;
                renderer.materials[i].color = Color.yellow;
            }
        }
        else
        {
            startColors[0][0] = renderer.material.color;
        }
            
        for (var i = 0; i < children.Length; i++)
        {
            startColors[i + 1] = new Color[children[i].materials.Length];
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                startColors[i+1][j] = children[i].materials[j].color;
                children[i].materials[j].color = Color.yellow;
            }
        }
    }
    
    public void unselectField()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer.materials.Length > 1)
        {
            for (var i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = startColors[0][i];
            }
        }
        else
        {
            renderer.material.color = startColors[0][0];
        }
        var children = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < children.Length; i++)
        {
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                children[i].materials[j].color = startColors[i+1][j];
            }
        }
    }
}
