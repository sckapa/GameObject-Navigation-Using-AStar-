using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int i = 0; i < obstacleData.obstacles.Length; i++)
        {
            if (obstacleData.obstacles[i])
            {
                int x = i % 10;
                int y = i / 10;
                Vector3 position = new Vector3(x * 1.1f, 0.5f, y * 1.1f);
                Instantiate(obstaclePrefab, position, Quaternion.identity);
            }
        }
    }
}