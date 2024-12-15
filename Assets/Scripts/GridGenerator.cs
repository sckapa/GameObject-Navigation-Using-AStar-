using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    private int gridSize = 10;
    private float tileSpacing = 1.1f; 

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.AddComponent<Tile>().SetPosition(x, y);
            }
        }
    }
}