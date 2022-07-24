using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public ComputeShader computeshader;

    private const int _maxRows = 1000;
    private const int _maxCols = 1000;

    private int[,] _grid;
    public int[,] grid
    {
        get
        {
            return _grid;
        }
    }

    private ComputeBuffer currentState;
    private ComputeBuffer nextState;

    string array_to_str(int[] array, int cols, int rows)
    {
        string s = "";
        for (int r = 0; r < rows; r += 1)
        {
            for (int c = 0; c < cols; c += 1)
            {
                s += array[r * cols + c] + " ";
            }
            s += '\n';
        }
        return s;
    }

    // Start is called before the first frame update
    void Start()
    {
        _grid = new int[_maxRows, _maxCols];

        for (int r = 0; r < _grid.GetLength(0); r += 1)
        {
            for (int c = 0; c < _grid.GetLength(1); c += 1)
            {
                _grid[r, c] = Random.Range(0, 2);
            }
        }

        currentState = new ComputeBuffer(_maxRows * _maxCols, sizeof(int));
        nextState = new ComputeBuffer(_maxRows * _maxCols, sizeof(int));

        currentState.SetData(_grid);
    }

    public void nextGrid()
    {
        lock (currentState) lock (nextState)
            {
                computeshader.SetInt("rows", _maxRows);
                computeshader.SetInt("cols", _maxCols);

                computeshader.SetBuffer(computeshader.FindKernel("CSMain"), "currentState", currentState);
                computeshader.SetBuffer(computeshader.FindKernel("CSMain"), "nextState", nextState);

                computeshader.Dispatch(computeshader.FindKernel("CSMain"), _maxCols / 8, _maxRows / 8, 1);

                nextState.GetData(_grid);

                (currentState, nextState) = (nextState, currentState);
            }
    }
}
