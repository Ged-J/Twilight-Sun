using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{

    [SerializeField] 
    private Tilemap floorTilemap , wallTilemap;
    
    [SerializeField]
    private TileBase floorTile , wallTop;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {

        PaintTiles(floorPos, floorTilemap, floorTile);

    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {

        foreach (var pos in positions)
        {

            PaintSingleTile(tilemap, tile, pos);

        }
        
    }


    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {

        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);

    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    public void PaintSingleBasicWall(Vector2Int position)
    {
        
        PaintSingleTile(wallTilemap, wallTop, position);
        
    }
}
