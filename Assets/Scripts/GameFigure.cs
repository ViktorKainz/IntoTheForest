using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum FigureType
{
    King,
    Pawn
}