using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour, AI
{
    public float moveSpeed = 2f;
    public static bool isMoving = false;
    private Vector3 targetPosition;
    private Pathfinding pathfinding;
    private Queue<Vector3> pathQueue;
    private Transform playerTransform;
    private Vector2Int currentGridPos;
    private ObstacleData obstacleData;

    private void Start()
    {
        transform.position = new Vector3(0, 1.5f, 0);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();

        pathfinding = new Pathfinding(obstacleManager.obstacleData);
        obstacleData = obstacleManager.obstacleData;

        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        playerController.OnPlayerMoved += OnPlayerMoved;

        currentGridPos = pathfinding.WorldToGrid(transform.position);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.0001f);
        obstacleData.SetObstacle(currentGridPos.x, currentGridPos.y, true);
    }

    private void OnDestroy()
    {
        obstacleData.SetObstacle(currentGridPos.x, currentGridPos.y, false);
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
    }

    private void OnPlayerMoved()
    {
        Move();
    }

    public void Move()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector2Int playerGridPos = pathfinding.WorldToGrid(playerPosition);

        if (IsAdjacentToPlayer(playerGridPos))
        {
            return;
        }

        List<Vector2Int> neighbors = pathfinding.GetNeighbors(playerGridPos);

        Vector2Int closestTile = playerGridPos;
        float closestDistance = float.MaxValue;

        foreach (Vector2Int neighbor in neighbors)
        {
            if (pathfinding.IsWalkable(neighbor.x, neighbor.y))
            {
                float distance = Vector3.Distance(transform.position, pathfinding.GridToWorld(neighbor));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTile = neighbor;
                }
            }
        }

        List<Vector3> path = pathfinding.FindPath(transform.position, pathfinding.GridToWorld(closestTile));
        if (path != null && path.Count > 1)
        {
            pathQueue = new Queue<Vector3>(path);
            pathQueue.Dequeue(); // Remove the current position
            isMoving = true;

            obstacleData.SetObstacle(currentGridPos.x, currentGridPos.y, false);
        }
    }

    private void MoveAlongPath()
    {
        if (pathQueue.Count > 0)
        {
            targetPosition = pathQueue.Peek();
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                pathQueue.Dequeue();
            }
        }
        else
        {
            isMoving = false;

            currentGridPos = pathfinding.WorldToGrid(transform.position);
            obstacleData.SetObstacle(currentGridPos.x, currentGridPos.y, true);
        }
    }

    private bool IsAdjacentToPlayer(Vector2Int playerGridPos)
    {
        List<Vector2Int> neighbors = pathfinding.GetNeighbors(currentGridPos);
        foreach (Vector2Int neighbor in neighbors)
        {
            if (neighbor == playerGridPos)
            {
                return true;
            }
        }
        return false;
    }
}