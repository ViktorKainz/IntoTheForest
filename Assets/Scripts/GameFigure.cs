using UnityEngine;

public class GameFigure : MonoBehaviour
{
    public bool enemy;
    public FigureType type;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum FigureType
{
    King, Pawn
}