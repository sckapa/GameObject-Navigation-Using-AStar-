using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    public bool[] obstacles = new bool[100];

    public void SetObstacle(int x, int y, bool isObstacle)
    {
        int index = y * 10 + x;
        obstacles[index] = isObstacle;
    }
}