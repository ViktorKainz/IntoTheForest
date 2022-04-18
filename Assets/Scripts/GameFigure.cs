using UnityEngine;
using UnityEngine.UI;

public class GameFigure : MonoBehaviour
{
    public bool enemy;
    public FigureType type;

    void Start()
    {
        var children = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < children.Length; i++)
        {
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                children[i].materials[j].color = enemy ? Color.red : Color.green;
            }
        }
    }

    public void Kill()
    {
        var children = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < children.Length; i++)
        {
            children[i].enabled = false;
        }
        var t = transform;
        var pos = t.position;
        pos.y = -1000;
        t.position = pos;
        if (type == FigureType.King)
        {
            var text = GameObject.FindWithTag("VictoryText").GetComponent<Text>();
            if (enemy)
            {
                text.color = Color.green;
                text.text = "Player green has won";
            }
            else
            {
                text.color = Color.red;
                text.text = "Player red has won";
            }
            TerrainField.round = -1;
        }
    } 
}

public enum FigureType
{
    King,
    Pawn
}