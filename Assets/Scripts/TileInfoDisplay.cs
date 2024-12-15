using TMPro;
using UnityEngine;

public class TileInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI infoText;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                infoText.text = $"Tile Position: ({tile.x}, {tile.y})";
            }
        }
    }
}