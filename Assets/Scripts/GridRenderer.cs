using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    private GameOfLife simulator;
    private GridManager gridManager;

    public Shader gridShader;
    private Material gridMaterial;

    public float _gridGap = 0.1f;
    public float gridGap
    {
        get { return _gridGap; }
        set
        {
            _gridGap = value;
            gridMaterial.SetFloat("gap", gridGap);
        }
    }

    public void setCurrentBuffer()
    {
        gridMaterial.SetBuffer("currentState", simulator.currentState);
    }

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        simulator = GetComponent<GameOfLife>();

        gridMaterial = new Material(gridShader);

        gridMaterial.SetInt("rows", GameOfLife.maxRows);
        gridMaterial.SetInt("cols", GameOfLife.maxCols);

        gridMaterial.SetFloat("gap", gridGap);

        setCurrentBuffer();

        GetComponent<Renderer>().material = gridMaterial;
    }
}
