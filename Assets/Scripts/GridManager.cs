using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class GridManager : MonoBehaviour, IPointerClickHandler
{
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

    private GameOfLife simulator;
    private GridRenderer gridRenderer;

    private RectTransform rectTransform;
    private Camera gridCamera;

    void Start()
    {
        gridRenderer = GetComponent<GridRenderer>();
        simulator = GetComponent<GameOfLife>();

        rectTransform = GetComponent<RectTransform>();
        gridCamera = GameObject.FindGameObjectWithTag("GridCamera").GetComponent<Camera>();

        playSpeed = 10;

        Invoke("nextGrid", 0);
    }

    public void OnPointerClick(PointerEventData data)
    {
        Vector2 localPoint = new Vector2(0, 0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, gridCamera, out localPoint);

        int r = Mathf.FloorToInt((localPoint.x + 0.5f) * GameOfLife.maxCols);
        int c = Mathf.FloorToInt((localPoint.y + 0.5f) * GameOfLife.maxRows);

        Debug.Log($"{r}, {c}");
        simulator.toggleCell(r, c);
    }

    private void nextGrid()
    {
        simulator.nextGrid();
        gridRenderer.setCurrentBuffer();
        Invoke("nextGrid", 1);
    }
}
