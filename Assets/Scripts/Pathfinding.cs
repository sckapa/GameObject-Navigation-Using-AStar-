using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private int gridSize = 10;
    private float tileSpacing = 1.1f;
    private ObstacleData obstacleData;

    public Pathfinding(ObstacleData obstacleData)
    {
        this.obstacleData = obstacleData;
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        Vector2Int startGridPos = WorldToGrid(start);
        Vector2Int endGridPos = WorldToGrid(end);

        List<Vector3> path = new List<Vector3>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = new Node(startGridPos, null, 0, GetDistance(startGridPos, endGridPos));
        Node endNode = new Node(endGridPos, null, 0, 0);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.gridPosition == endNode.gridPosition)
            {
                Node current = currentNode;
                while (current != null)
                {
                    path.Add(GridToWorld(current.gridPosition));
                    current = current.parent;
                }
                path.Reverse();
                return path;
            }

            foreach (Vector2Int neighborPosition in GetNeighbors(currentNode.gridPosition))
            {
                if (!IsWalkable(neighborPosition.x, neighborPosition.y) || closedList.Contains(new Node(neighborPosition)))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode.gridPosition, neighborPosition);
                Node neighborNode = new Node(neighborPosition, currentNode, newMovementCostToNeighbor, GetDistance(neighborPosition, endGridPos));

                if (newMovementCostToNeighbor < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = newMovementCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighborPosition, endGridPos);
                    neighborNode.parent = currentNode;

                    openList.Add(neighborNode);
                }
            }
        }

        return path;
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tileSpacing);
        int y = Mathf.RoundToInt(worldPosition.z / tileSpacing);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * tileSpacing, 1.5f, gridPosition.y * tileSpacing);
    }

    private int GetDistance(Vector2Int a, Vector2Int b)
    {
        int dstX = Mathf.Abs(a.x - b.x);
        int dstY = Mathf.Abs(a.y - b.y);
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public List<Vector2Int> GetNeighbors(Vector2Int gridPosition)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Horizontal and Vertical neighbors
        if (gridPosition.x - 1 >= 0) neighbors.Add(new Vector2Int(gridPosition.x - 1, gridPosition.y));
        if (gridPosition.x + 1 < gridSize) neighbors.Add(new Vector2Int(gridPosition.x + 1, gridPosition.y));
        if (gridPosition.y - 1 >= 0) neighbors.Add(new Vector2Int(gridPosition.x, gridPosition.y - 1));
        if (gridPosition.y + 1 < gridSize) neighbors.Add(new Vector2Int(gridPosition.x, gridPosition.y + 1));

        // Diagonal neighbors
        if (gridPosition.x - 1 >= 0 && gridPosition.y - 1 >= 0) neighbors.Add(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1));
        if (gridPosition.x + 1 < gridSize && gridPosition.y - 1 >= 0) neighbors.Add(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1));
        if (gridPosition.x - 1 >= 0 && gridPosition.y + 1 < gridSize) neighbors.Add(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1));
        if (gridPosition.x + 1 < gridSize && gridPosition.y + 1 < gridSize) neighbors.Add(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1));

        return neighbors;
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
        {
            return false;
        }

        int index = y * gridSize + x;
        return !obstacleData.obstacles[index];
    }

    private class Node
    {
        public Vector2Int gridPosition;
        public Node parent;
        public int gCost;
        public int hCost;
        public int FCost { get { return gCost + hCost; } }

        public Node(Vector2Int gridPosition, Node parent = null, int gCost = 0, int hCost = 0)
        {
            this.gridPosition = gridPosition;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }
    }
}