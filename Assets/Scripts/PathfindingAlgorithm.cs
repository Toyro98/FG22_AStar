using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfindingAlgorithm : MonoBehaviour
{
    public int width = 48;
    public int height = 24;
    public float cellSize = 16f;
    public MyGrid grid;

    private Node _start;
    private Node _end;

    private void Start()
    {
        grid = new MyGrid(width, height, cellSize);

        // Set camera position to be in the middle and set size so you can see the entire grid
        Camera.main.transform.position = new Vector3(grid.width * grid.cellSize / 2, grid.height * grid.cellSize / 2, -10);
        Camera.main.orthographicSize = (grid.height * grid.cellSize / 2) + 64;
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

            _start = new Node(grid.GetGridXY(vec));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!grid.IsInsideGrid(grid.GetGridXY(vec)))
            {
                return;
            }

            _end = new Node(grid.GetGridXY(vec));
            AStar(_start, _end);
        }
    }

    private static readonly List<Vector2> _directions = new List<Vector2>
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    public class Node
    {
        public Vector2 location;
        public float g;
        public float h;
        public float f;
        public Node parent;

        public Node() {}

        public Node(Vector2 location)
        {
            this.location = location;
        }

        public Node(Vector2 location, float g, float h, float f, Node parent)
        {
            this.location = location;

            this.g = g;
            this.h = h;
            this.f = f;
            this.parent = parent;
        } 
    }

    public void AStar(Node start, Node goal)
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        start.g = 1;
        start.h = Vector2.Distance(start.location, goal.location);
        start.f = start.g + start.h;

        open.Add(start); 
        Node currentNode = start;

        while (open.Count > 0)
        {
            if (currentNode == goal)
            {
                break;
            }

            foreach (var direction in _directions)
            {
                Node neighbour = new Node();
                neighbour.location = direction + currentNode.location;

                if (grid.GetGridInfo(neighbour.location) == Path.Wall)
                {
                    continue;
                }

                foreach (var item in closed)
                {
                    if (neighbour.location == item.location)
                    {
                        continue;
                    }
                }

                float g = currentNode.g + 1;
                float h = Vector2.Distance(neighbour.location, goal.location);
                float f = g + h;

                //foreach (var item in open)
                //{
                //    if (neighbour.location == item.location)
                //    {
                //        item.g = g;
                //        item.h = h;
                //        item.f = f;
                //        item.parent = currentNode;
                //        continue;
                //    }
                //}

                open.Add(new Node(neighbour.location, g, h, f, currentNode));
            }

            open = open.OrderBy(x => x.f).ToList();
            Node n = open.FirstOrDefault();

            closed.Add(n);
            open.RemoveAt(0);

            currentNode = n;
        }
    }
}
