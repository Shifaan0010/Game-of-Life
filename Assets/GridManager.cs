using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tile;

    [Range(0.1f, 2)]
    public float tileSize = 1;

    [Range(0, 1)]
    public float tileGap = 0.1f;

    public int rows
    {
        get
        {
            return Mathf.CeilToInt(Camera.main.orthographicSize * 2 / (tileSize + tileGap)) + 1;
        }
    }

    public int cols
    {
        get
        {
            return Mathf.CeilToInt(Camera.main.orthographicSize * 2 * Camera.main.aspect / (tileSize + tileGap)) + 1;
        }
    }

    private List<List<GameObject>> grid;

    bool _paused;
    public bool paused
    {
        get
        {
            return _paused;
        }
        set
        {
            _paused = value;
            Time.timeScale = (value) ? 0 : playSpeed;
        }
    }

    float _playSpeed;
    public float playSpeed
    {
        get
        {
            return _playSpeed;
        }
        set
        {
            _playSpeed = value;

            if (!_paused)
            {
                Time.timeScale = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new List<List<GameObject>>();
        resizeGrid(rows, cols);

        playSpeed = 10;

        Invoke("nextGrid", 0);
    }

    private void nextGrid()
    {
        nextState();
        Invoke("nextGrid", 1);
    }

    private Vector3 calcPosition(int row, int col)
    {
        return new Vector3((col - cols / 2) * (tileSize + tileGap), (row - rows / 2) * (tileSize + tileGap), 0);
    }

    private void calcAllPositions()
    {
        for (int row = 0; row < rows; row += 1)
        {
            for (int col = 0; col < cols; col += 1)
            {
                grid[row][col].transform.position = calcPosition(row, col);
                grid[row][col].transform.localScale = Vector3.one * tileSize;
            }
        }
    }

    void resizeGrid(int newRows, int newCols)
    {
        lock (grid)
        {
            while (grid.Count != newRows)
            {
                if (grid.Count < newRows)
                {
                    grid.Add(new List<GameObject>());
                }
                else
                {
                    List<GameObject> list = grid[grid.Count - 1];

                    foreach (GameObject g in list)
                    {
                        Destroy(g);
                    }

                    grid.RemoveAt(grid.Count - 1);
                }
            }

            for (int row = 0; row < grid.Count; row += 1)
            {
                while (grid[row].Count != newCols)
                {
                    if (grid[row].Count < newCols)
                    {
                        grid[row].Add(Instantiate(tile));
                    }
                    else
                    {
                        GameObject g = grid[row][grid[row].Count - 1];

                        Destroy(g);

                        grid[row].RemoveAt(grid[row].Count - 1);
                    }
                }
            }

            calcAllPositions();
        }
    }

    private bool isAlive(int row, int col)
    {
        return grid[row][col].GetComponent<StateManager>().isAlive;
    }

    void nextState()
    {
        if (grid.Count == 0)
        {
            return;
        }

        lock (grid)
        {
            int currentRows = grid.Count, currentCols = grid[0].Count;
            
            bool[,] nextGridState = new bool[currentRows, currentCols];

            for (int row = 0; row < currentRows; row += 1)
            {
                for (int col = 0; col < currentCols; col += 1)
                {
                    int aliveNeighborCount = 0;

                    (int, int)[] neighborPositions = {
                    (row + 1, col), (row + 1, col + 1), (row + 1, col - 1), (row, col + 1), (row, col - 1), (row - 1, col - 1), (row - 1, col), (row - 1, col + 1) };

                    foreach ((int neighborRow, int neighborCol) in neighborPositions)
                    {
                        try
                        {
                            if (isAlive(neighborRow, neighborCol))
                            {
                                aliveNeighborCount += 1;
                            }
                        }
                        catch (ArgumentOutOfRangeException e)
                        {

                        }
                    }

                    if (aliveNeighborCount < 2 || aliveNeighborCount > 3)
                    {
                        nextGridState[row, col] = false;
                    }
                    else if (aliveNeighborCount == 3)
                    {
                        nextGridState[row, col] = true;
                    }
                    else
                    {
                        nextGridState[row, col] = isAlive(row, col);
                    }
                }
            }

            string matrixStr = "";
            for (int i = 0; i < currentRows; i++)
            {
                for (int j = 0; j < currentCols; j++)
                {
                    matrixStr += string.Format("{0} ", nextGridState[i, j] ? 1 : 0);
                }
                matrixStr += Environment.NewLine + Environment.NewLine;
            }
            //Debug.Log(matrixStr);

            setGridState(nextGridState);
        }
    }

    private void setGridState(bool[,] state)
    {
        for (int row = 0; row < state.GetLength(0); row += 1)
        {
            for (int col = 0; col < state.GetLength(1); col += 1)
            {
                //Debug.Log($"{row} {col} {grid[row][col].GetComponent<StateManager>().isAlive}");
                grid[row][col].GetComponent<StateManager>().isAlive = state[row, col];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rows != grid.Count || (grid.Count > 0 && cols != grid[0].Count))
        {
            Debug.Log($"Resize {rows} {cols}");
            resizeGrid(rows, cols);
        }
    }
}
