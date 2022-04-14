using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using static TerrainField;

public enum Team
{
    Red,
    Green,
    White
}

public class SpawnFigure : MonoBehaviour
{
    public GameObject canvas;
    
    private Team team;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("canvas");
        canvas = Instantiate(canvas);
        canvas.GetComponent<SpawnFigureCanvas>().castle = gameObject;
    }

    private void OnMouseUp()
    {
        if ((round % 2 == 0 && team == Team.Red) ||
            (round % 2 == 1 && team == Team.Green))
        {
            canvas.SetActive(true);
        }
    }

    public void setInactive()
    {
        canvas.SetActive(false);
    }
    
    public Team getTeam()
    {
        return team;
    }

    public void setTeam(Team newTeam)
    {
        team = newTeam;
    }
}

