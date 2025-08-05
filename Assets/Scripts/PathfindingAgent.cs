using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfindingAgent : MonoBehaviour
{
    private GridManager gridManager;
    private List<Vector2Int> currentPath;
    private HashSet<Vector2Int> visitedNodes;

    public void Initialize(GridManager manager)
    {
        gridManager = manager;
    }

    public void FindAndShowPath()
    {
        if (gridManager == null) return;

        Vector2Int start = gridManager.GetNPCPosition();
        Vector2Int goal = gridManager.GetGoalPosition();

        currentPath = FindPath(start, goal);

        if (currentPath != null && currentPath.Count > 0)
        {
            Debug.Log($"Path found! Steps: {currentPath.Count}");
            for (int i = 0; i < currentPath.Count; i++)
            {
                Debug.Log($"Step {i}: {currentPath[i]}");
            }

            gridManager.HighlightPath(currentPath);
        }
        else
        {
            ShowText.Instance.ShowTextTu("No path found!");
            ShowText.Instance.HideTextTu(1f);
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        var openSet = new List<Node>();
        var closedSet = new HashSet<Vector2Int>();
        visitedNodes = new HashSet<Vector2Int>();

        var startNode = new Node(start, 0, GetHeuristic(start, goal), null);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet.OrderBy(n => n.FCost).ThenBy(n => n.HCost).First();
            openSet.Remove(currentNode);
            closedSet.Add(currentNode.Position);
            visitedNodes.Add(currentNode.Position);

            if (currentNode.Position == goal)
            {
                return ReconstructPath(currentNode);
            }

            foreach (var neighbor in GetNeighbors(currentNode.Position))
            {
                if (closedSet.Contains(neighbor) || !gridManager.IsWalkable(neighbor.x, neighbor.y))
                    continue;

                float gCost = currentNode.GCost + 1;
                float hCost = GetHeuristic(neighbor, goal);

                var existingNode = openSet.FirstOrDefault(n => n.Position == neighbor);

                if (existingNode == null)
                {
                    openSet.Add(new Node(neighbor, gCost, hCost, currentNode));
                }
                else if (gCost < existingNode.GCost)
                {
                    existingNode.GCost = gCost;
                    existingNode.Parent = currentNode;
                }
            }
        }

        return null; 
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        var neighbors = new List<Vector2Int>();

        Vector2Int[] directions = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = position + dir;
            if (gridManager.IsValidPosition(neighbor.x, neighbor.y))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> ReconstructPath(Node goalNode)
    {
        var path = new List<Vector2Int>();
        var currentNode = goalNode;

        while (currentNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    public List<Vector2Int> GetCurrentPath()
    {
        return currentPath;
    }
}
