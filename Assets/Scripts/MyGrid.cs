using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid
{
    public int width;
    public int height;
    public float cellSize;
    private Path[,] _grid;

    public MyGrid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        _grid = new Path[width, height];

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPositon(x, y), GetWorldPositon(x, y + 1), Color.white, Mathf.Infinity);
                Debug.DrawLine(GetWorldPositon(x, y), GetWorldPositon(x + 1, y), Color.white, Mathf.Infinity);
            }
        }

        Debug.DrawLine(GetWorldPositon(0, height), GetWorldPositon(width, height), Color.white, Mathf.Infinity);
        Debug.DrawLine(GetWorldPositon(width, 0), GetWorldPositon(width, height), Color.white, Mathf.Infinity);

        SetRandomWalls();
        SetWalls();
    }

    private Vector2Int GetXY(Vector3 worldPositon)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPositon.x / cellSize), Mathf.FloorToInt(worldPositon.y / cellSize));
    }

    private Vector3 GetWorldPositon(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetValue(int x, int y, Path value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            _grid[x, y] = value;
        }
    }

    private void SetRandomWalls()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                SetValue(x, y, Random.Range(0, 100) < 40 ? Path.Wall : Path.Empty);
            }
        }
    }

    private void SetWalls()
    {
        for (int x = 0; x < width; x++)
        {
            SetValue(x, 0, Path.Wall); 
            SetValue(x, height - 1, Path.Wall);
        }

        for (int y = 0; y < height; y++)
        {
            SetValue(0, y, Path.Wall);
            SetValue(width - 1, y, Path.Wall);
        }
    }

    public void SetValue(Vector3 worldPositon, Path value)
    {
        Vector2Int pos = GetXY(worldPositon);
        SetValue(pos.x, pos.y, value);
    }
}
