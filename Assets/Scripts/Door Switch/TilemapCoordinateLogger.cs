using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCoordinateLogger : MonoBehaviour
{
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(worldPos);
            Debug.Log("Clicked Grid Position: " + gridPos);
        }
    }
}