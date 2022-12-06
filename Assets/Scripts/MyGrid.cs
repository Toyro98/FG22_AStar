using UnityEngine;

[System.Serializable]
public class MyGrid
{
    public int width;
    public int height;
    public Transform gridParent;

    [Range(0.05f, 0.5f)]
    [Tooltip("If it's lower than the current value, it will create a wall")]
    public float createWallOdds = 0.3f;

    private readonly Path[,] _grid;
    private readonly GameObject[,] _cubes;

    public MyGrid(MyGrid grid)
    {
        width = grid.width;
        height = grid.height;
        createWallOdds = grid.createWallOdds;
        gridParent = grid.gridParent;

        _grid = new Path[width, height];
        _cubes = new GameObject[width, height];

        SetBorderWalls();
        DrawGridFloor();
        SetRandomWalls();
    }

    public Vector2Int GetGridXY(Vector3 worldPositon)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPositon.x), Mathf.FloorToInt(worldPositon.y));
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
                SetGridType(x, y, _grid[x, y]);
            }
        }
    }

    public void SetGridType(int x, int y, Path value)
    {
        if (IsInsideGrid(x, y))
        {
            _grid[x, y] = value;

            if (_cubes[x, y] == null)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.name = $"Grid [{x},{y}]";
                obj.transform.SetParent(gridParent);
                obj.transform.position = new Vector3(x, y) + new Vector3(.5f, .5f, 0);

                _cubes[x, y] = obj;
            }

            _cubes[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load(value == Path.Wall ? "wall" : "empty");
        }
    }

    private void SetRandomWalls()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (Random.value < createWallOdds)
                {
                    SetGridType(x, y, Path.Wall);
                }
            }
        }

        SetGridType(1, 1, Path.Empty);
        SetGridType(1, 2, Path.Empty);
        SetGridType(2, 1, Path.Empty);
    }

    public void SetBorderWalls()
    {
        for (int x = 0; x < width; x++)
        {
            SetGridType(x, 0, Path.Wall);
            SetGridType(x, height - 1, Path.Wall);
        }

        for (int y = 0; y < height; y++)
        {
            SetGridType(0, y, Path.Wall);
            SetGridType(width - 1, y, Path.Wall);
        }
    }
}
