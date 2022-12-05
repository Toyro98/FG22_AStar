using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PathfindingAlgorithm _algorithm;
    [SerializeField] GameObject _test;

    [Range(1f, 10f)] public float playerSpeed = 3f;
    public Color lineColor;
    public List<Node> currentPath = new List<Node>();

    private Node _startNode;
    private Node _endNode;

    private void Update()
    {
        MovePlayer();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!_algorithm.grid.IsInsideGrid(_algorithm.grid.GetGridXY(vec)))
            {
                return;
            }

            _startNode = new Node(new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)));
            _endNode = new Node(_algorithm.grid.GetGridXY(vec));
            
            currentPath = _algorithm.AStar(_startNode, _endNode);

            if (currentPath.Count == 0)
            {
                Debug.Log("A* could not find a path!");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 cameraVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!_algorithm.grid.IsInsideGrid(_algorithm.grid.GetGridXY(cameraVector)))
            {
                return;
            }

            Vector2Int gridVector = _algorithm.grid.GetGridXY(cameraVector);

            if (_algorithm.grid.GetGridInfo(gridVector) == Path.Wall)
            {
                _algorithm.grid.SetValue(gridVector.x, gridVector.y, Path.Empty);
            }
            else
            {
                _algorithm.grid.SetValue(gridVector.x, gridVector.y, Path.Wall);
            }
        }
    }

    private void MovePlayer()
    {
        if (currentPath.Count != 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPath[0].position, playerSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentPath[0].position) < 0.01f)
            {
                currentPath.RemoveAt(0);
            }

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Vector2 a = new Vector2(currentPath[i].position.x + 0.5f, currentPath[i].position.y + 0.5f) * _algorithm.grid.cellSize;
                Vector2 b = new Vector2(currentPath[i + 1].position.x + 0.5f, currentPath[i + 1].position.y + 0.5f) * _algorithm.grid.cellSize;

                Debug.DrawLine(a, b, lineColor, Time.deltaTime);
            }
        }
    }
}
