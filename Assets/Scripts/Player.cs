using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PathfindingAlgorithm _algorithm;

    [Range(1f, 10f)] 
    public float playerSpeed = 5f;
    public Color lineColor;
    public List<Node> currentPath = new List<Node>();

    private Node _startNode;
    private Node _endNode;
    private bool _updateCurrentPath = false;
    private bool _gridLayoutChanged = false;

    private const float MinimumDistance = 0.01f;
    private const float Offset = 0.5f;

    private void Start()
    {
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
    }

    private void Update()
    {
        MoveCube();

        if (Input.GetMouseButtonDown(0))
        {
            SetNewEndNode();
        }

        if (Input.GetMouseButtonDown(1))
        {
            ChangeGridType();
        }
    }

    private void SetNewEndNode()
    {
        Vector3 cameraVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!_algorithm.grid.IsInsideGrid(_algorithm.grid.GetGridXY(cameraVector)))
        {
            return;
        }

        _endNode = new Node(_algorithm.grid.GetGridXY(cameraVector));

        if (currentPath.Count == 0)
        {
            _startNode = new Node(new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)));
            currentPath = _algorithm.AStar(_startNode, _endNode);
        }
        else
        {
            _updateCurrentPath = true;
        }

        if (currentPath.Count == 0)
        {
            Debug.Log("A* could not find a path!");
        }
    }

    private void ChangeGridType()
    {
        Vector3 cameraVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!_algorithm.grid.IsInsideGrid(_algorithm.grid.GetGridXY(cameraVector)))
        {
            return;
        }

        Vector2Int gridVector = _algorithm.grid.GetGridXY(cameraVector);

        if (_algorithm.grid.GetGridInfo(gridVector) == Path.Wall)
        {
            _algorithm.grid.SetGridType(gridVector.x, gridVector.y, Path.Empty);
        }
        else
        {
            _algorithm.grid.SetGridType(gridVector.x, gridVector.y, Path.Wall);
        }

        _gridLayoutChanged = true;
    }

    private void MoveCube()
    {
        if (currentPath.Count != 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPath[0].position, playerSpeed * Time.deltaTime);
            
            if (_updateCurrentPath || _gridLayoutChanged)
            {
                if (Vector2.Distance(transform.position, currentPath[0].position) < MinimumDistance)
                {
                    currentPath = new List<Node>(_algorithm.AStar(new Node(currentPath[0].position), _endNode));
                    _updateCurrentPath = false;
                    _gridLayoutChanged = false;
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, currentPath[0].position) < MinimumDistance)
                {
                    currentPath.RemoveAt(0);
                }
            }

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                DrawDebugLine(currentPath[i].position, currentPath[i + 1].position);
            }

            if (currentPath.Count != 0)
            {
                DrawDebugLine(transform.position, currentPath[0].position);
            }
        }
    }

    private void DrawDebugLine(Vector2 start, Vector2 end)
    {
        Debug.DrawLine(new Vector2(start.x + Offset, start.y + Offset), new Vector2(end.x + Offset, end.y + Offset), lineColor, Time.deltaTime);
    }
}
