using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    private GameOfLife simulator;
    private GridManager gridManager;

    public Shader gridShader;
    private Material gridMaterial;

    public void setCurrentBuffer() {
        gridMaterial.SetBuffer("currentState", simulator.currentState);
    }

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        simulator = GetComponent<GameOfLife>();

        gridMaterial = new Material(gridShader);
        
        gridMaterial.SetInt("rows", GameOfLife.maxRows);
        gridMaterial.SetInt("cols", GameOfLife.maxCols);

        setCurrentBuffer();

        GetComponent<Renderer>().material = gridMaterial;
    }

    void Update()
    {
        
    }
}
