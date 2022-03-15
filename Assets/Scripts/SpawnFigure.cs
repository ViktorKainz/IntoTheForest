using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SpawnFigure : MonoBehaviour
{
    public GameObject canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = Instantiate(canvas);
        canvas.GetComponent<SpawnFigureCanvas>().castle = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    public void setInactive()
    {
        canvas.SetActive(false);
    }
}
