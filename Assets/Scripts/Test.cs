using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int width = 48;
    public int height = 24;
    public float cellSize = 16f;
    private MyGrid _grid;

    private void Start()
    {
        _grid = new MyGrid(width, height, cellSize);

        // Set camera position to be in the middle and set size so you can see the entire grid
        Camera.main.transform.position = new Vector3(_grid.width * _grid.cellSize / 2, _grid.height * _grid.cellSize / 2, -10);
        Camera.main.orthographicSize = (_grid.height * _grid.cellSize / 2) + 64;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _grid.SetValue(vector, Path.Wall);
        }
    }
}
