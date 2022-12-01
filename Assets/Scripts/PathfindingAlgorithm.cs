using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAlgorithm : MonoBehaviour
{
    public MyGrid grid;

    public List<Node> open = new List<Node>();
    public List<Node> closed = new List<Node>();
    public List<Node> path = new List<Node>();

    private Node _startNode;
    private Node _endNode;

    private void Start()
    {
        grid = new MyGrid(grid.width, grid.height, grid.cellSize);

        // Set camera position to be in the middle and set size so you can see the entire grid
        Camera.main.transform.position = new Vector3(grid.width * grid.cellSize / 2, grid.height * grid.cellSize / 2, -10);
        Camera.main.orthographicSize = (grid.height * grid.cellSize / 2) + 64;

        _startNode = new Node(Vector2.one);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!grid.IsInsideGrid(grid.GetGridXY(vec)))
            {
                return;
            }

            _endNode = new Node(grid.GetGridXY(vec));
            List<Node> t = AStar(_startNode, _endNode);

            if (t.Count == 0)
            {
                Debug.Log("A* could not find a path!");
                return;
            }

            for (int i = 0; i < t.Count - 1; i++)
            {
                Vector2 a = new Vector2(t[i].location.x + 0.5f, t[i].location.y + 0.5f) * grid.cellSize;
                Vector2 b = new Vector2(t[i + 1].location.x + 0.5f, t[i + 1].location.y + 0.5f) * grid.cellSize;

                Debug.DrawLine(a, b, Color.green, 1f);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!grid.IsInsideGrid(grid.GetGridXY(vec)))
            {
                return;
            }

            Vector2Int v = grid.GetGridXY(vec);
            //Debug.Log(grid.GetGridInfo(v));

            if (grid.GetGridInfo(v) == Path.Wall)
            {
                grid.SetValue(v.x, v.y, Path.Empty);
            }
            else
            {
                grid.SetValue(v.x, v.y, Path.Wall);
            }
        }
    }

    private static readonly List<Vector2> _directions = new List<Vector2>
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    public List<Node> AStar(Node start, Node goal)
    {
        closed = new List<Node>();
        open = new List<Node> { start };

        start.g = 0;
        start.h = Vector2.Distance(start.location, goal.location);
        start.f = start.g + start.h;

        while (open.Count > 0)
        {
            Node current = GetLowestFNode(open);

            if (current.location == goal.location)
            {
                goal.parent = current;
                return GetPath(goal);
            }

            open.Remove(current);
            closed.Add(current);

            foreach (var direction in _directions)
            {
                Node neighbor = new Node
                {
                    location = current.location + direction
                };

                if (NeighborExistInList(closed, neighbor))
                {
                    continue;
                }

                if (grid.GetGridInfo(neighbor.location) != Path.Wall)
                {
                    neighbor.g = current.g + 1;
                    neighbor.h = Vector2.Distance(neighbor.location, goal.location);
                    neighbor.f = neighbor.g + neighbor.h;
                    neighbor.parent = current;

                    if (!NeighborExistInList(open, neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }

        return new List<Node>();
    }

    private bool NeighborExistInList(List<Node> nodes, Node current)
    {
        foreach (var node in nodes)
        {
            if (current.location == node.location)
            {
                return true;
            }
        }

        return false;
    }

    private Node GetLowestFNode(List<Node> nodes)
    {
        Node lowest = nodes[0];

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].f < lowest.f)
            {
                lowest = nodes[i];
            }
        }

        return lowest;
    }

    private List<Node> GetPath(Node end)
    {
        path = new List<Node>
        {
            end
        };

        Node current = end;

        while (current.parent != null)
        {
            path.Add(current.parent);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}
