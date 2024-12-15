using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Pathfinding pathfinding;
    private Queue<Vector3> pathQueue;

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler OnPlayerMoved;

    private void Start()
    {
        transform.position = new Vector3(0, 1.5f, 0);
        gameObject.tag = "Player";

        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();
        pathfinding = new Pathfinding(obstacleManager.obstacleData);
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
        else if (!EnemyAI.isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null)
                    {
                        Vector3 destination = new Vector3(tile.x * 1.1f, 1.5f, tile.y * 1.1f);
                        List<Vector3> path = pathfinding.FindPath(transform.position, destination);
                        if (path != null && path.Count > 0)
                        {
                            pathQueue = new Queue<Vector3>(path);
                            isMoving = true;
                        }
                    }
                }
            }
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
            OnPlayerMoved?.Invoke();
        }
    }
}