using System.Collections.Generic;
using UnityEngine;

public class PathfindingAlgorithm : MonoBehaviour
{
    public MyGrid grid;
    public List<Node> open = new List<Node>();
    public List<Node> closed = new List<Node>();

    private void Start()
    {
        grid = new MyGrid(grid.width, grid.height, grid.cellSize, transform);

        // Set camera position to be in the middle and set size so you can see the entire grid
        Camera.main.transform.position = new Vector3(grid.width * grid.cellSize / 2, grid.height * grid.cellSize / 2, -10);
        Camera.main.orthographicSize = 16;
        //Camera.main.orthographicSize = (grid.height * grid.cellSize / 2) + 64;
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
        start.h = Vector2.Distance(start.position, goal.position);
        start.f = start.g + start.h;

        while (open.Count > 0)
        {
            Node current = GetLowestFNode(open);

            if (current.position == goal.position)
            {
                goal.parent = current;
                return GetPath(goal);
            }

            open.Remove(current);
            closed.Add(current);

            foreach (var direction in _directions)
            {
                Node neighbor = new Node(current.position + direction);

                if (NeighborExistInList(closed, neighbor))
                {
                    continue;
                }

                if (grid.GetGridInfo(neighbor.position) != Path.Wall)
                {
                    neighbor.g = current.g + 1;
                    neighbor.h = Vector2.Distance(neighbor.position, goal.position);
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
            if (current.position == node.position)
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
        List<Node> path = new List<Node>
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
