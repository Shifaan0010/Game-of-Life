using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public ComputeShader computeshader;

    public const int maxRows = 1000;
    public const int maxCols = 1000;

    private int[,] _grid;

    private ComputeBuffer _currentState;
    public ComputeBuffer currentState { get { return _currentState; } }

    private ComputeBuffer _nextState;

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

    void Awake()
    {
        _grid = new int[maxRows, maxCols];

        _currentState = new ComputeBuffer(maxRows * maxCols, sizeof(int));
        _nextState = new ComputeBuffer(maxRows * maxCols, sizeof(int));
    }

    void Start()
    {
        for (int r = 0; r < _grid.GetLength(0); r += 1)
        {
            for (int c = 0; c < _grid.GetLength(1); c += 1)
            {
                _grid[r, c] = Random.Range(0, 2);
            }
        }

        _currentState.SetData(_grid);
    }

    public void toggleCell(int r, int c)
    {
        lock (_currentState) lock (_nextState)
            {
                _currentState.GetData(_grid);
                _grid[r, c] = 1 - _grid[r, c];
                _currentState.SetData(_grid);
            }
    }

    public void nextGrid()
    {
        lock (_currentState) lock (_nextState)
            {
                computeshader.SetInt("rows", maxRows);
                computeshader.SetInt("cols", maxCols);

                computeshader.SetBuffer(computeshader.FindKernel("CSMain"), "currentState", _currentState);
                computeshader.SetBuffer(computeshader.FindKernel("CSMain"), "nextState", _nextState);

                computeshader.Dispatch(computeshader.FindKernel("CSMain"), Mathf.CeilToInt(maxCols / 8.0f), Mathf.CeilToInt(maxRows / 8.0f), 1);

                // _nextState.GetData(_grid);

                (_currentState, _nextState) = (_nextState, _currentState);
            }
    }

    void OnDestroy()
    {
        _currentState.Release();
        _nextState.Release();
    }
}
