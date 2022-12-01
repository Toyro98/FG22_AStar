using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2 location;
    public float g;
    public float h;
    public float f;
    public Node parent;

    public Node() { }

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
