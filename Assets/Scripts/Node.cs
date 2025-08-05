using UnityEngine;

public class Node
{
    public Vector2Int Position { get; set; }
    public float GCost { get; set; } 
    public float HCost { get; set; } 
    public float FCost => GCost + HCost; 
    public Node Parent { get; set; }

    public Node(Vector2Int position, float gCost, float hCost, Node parent)
    {
        Position = position;
        GCost = gCost;
        HCost = hCost;
        Parent = parent;
    }
}
