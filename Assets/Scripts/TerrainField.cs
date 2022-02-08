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
        var children = GetComponentsInChildren<Renderer>();
        startColors = new Color[children.Length][];
        for (var i = 0; i < children.Length; i++)
        {
            startColors[i] = new Color[children[i].materials.Length];
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                startColors[i][j] = children[i].materials[j].color;
                children[i].materials[j].color = Color.yellow;
            }
        }
    }
    
    public void unselectField()
    {
        var children = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < children.Length; i++)
        {
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                children[i].materials[j].color = startColors[i][j];
            }
        }
    }
}
