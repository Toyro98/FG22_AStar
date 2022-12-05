using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2 position;
    public float g;
    public float h;
    public float f;
    public Node parent;

    public Node(Vector2 position)
    {
        this.position = position;
    }
}
