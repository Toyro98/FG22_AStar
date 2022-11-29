using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfindingAlgorithm
{
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

        open.Add(start);
        Node currentNode = start;

        while (true)
        {
            if (currentNode == goal)
            {
                break;
            }

            foreach (var direction in _directions)
            {
                Node neighbour = new Node()
                {
                    location = direction + currentNode.location
                };

                float g = Vector2.Distance(currentNode.location, neighbour.location) + currentNode.g;
                float h = Vector2.Distance(neighbour.location, goal.location);
                float f = g + h;

                foreach (var item in open)
                {
                    if (currentNode.location == item.location)
                    {
                        item.g = g;
                        item.h = h;
                        item.f = f;
                        item.parent = currentNode;
                        continue;
                    }
                }

                for (int i = 0; i < open.Count; i++)
                {
                    if (currentNode.location == open[i].location)
                    {
                        open[i].g = g;
                        open[i].h = h;
                        open[i].f = f;
                        open[i].parent = currentNode;
                        continue;
                    }
                }

                open.Add(new Node(currentNode.location, g, h, f, neighbour));
            }

            open = open.OrderBy(x => x.f).ToList();
            Node n = open.FirstOrDefault();

            closed.Add(n);
            open.RemoveAt(0);

            currentNode = n;
        }
    }
}
