using UnityEngine;

[System.Serializable]
public class MyGrid
{
    public int width;
    public int height;
    public float cellSize;
    public Transform transform;

    private Path[,] _grid;
    private GameObject[,] _cubes;

    public MyGrid(int width, int height, float cellSize, Transform transform)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.transform = transform;

        _grid = new Path[width, height];
        _cubes = new GameObject[width, height];

        SetWalls();
        DrawGridFloor();
        SetRandomWalls();
    }

    public Vector2Int GetGridXY(Vector3 worldPositon)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPositon.x / cellSize), Mathf.FloorToInt(worldPositon.y / cellSize));
    }

    private Vector3 GetWorldPositon(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public Path GetGridInfo(int x, int y)
    {
        return _grid[x, y];
    }

    public Path GetGridInfo(Vector2 vector)
    {
        return GetGridInfo((int)vector.x, (int)vector.y);
    }

    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool IsInsideGrid(Vector2 vector)
    {
        Vector2 v = GetGridXY(vector);
        return IsInsideGrid((int)v.x, (int)v.y);
    }

    public void DrawGridFloor()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                SetValue(x, y, _grid[x, y]);
            }
        }
    }

    public void SetValue(int x, int y, Path value)
    {
        if (IsInsideGrid(x, y))
        {
            _grid[x, y] = value;

            if (_cubes[x, y] == null)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.name = $"Grid [{x},{y}]";
                obj.transform.SetParent(transform);
                obj.transform.localScale = new Vector3(cellSize, cellSize, 0);
                obj.transform.position = GetWorldPositon(x, y) + new Vector3(cellSize / 2, cellSize / 2, 0);

                _cubes[x, y] = obj;
            }

            if (value == Path.Wall)
            {
                _cubes[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("wall");
            }
            else
            {
                _cubes[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("empty");
            }
        }
    }

    private void SetRandomWalls()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (Random.Range(0, 100) < 30)
                {
                    SetValue(x, y, Path.Wall);
                }
            }
        }

        SetValue(1, 1, Path.Empty);
        SetValue(1, 2, Path.Empty);
        SetValue(2, 1, Path.Empty);
    }

    public void SetWalls()
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
}
