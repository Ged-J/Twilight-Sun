using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    static WallGenerator()
    {
    }

    public static void CreateWalls(HashSet<Vector2Int> floorPos, TilemapVisualizer tilemapVisualizer)
    {
        
        var basicWallPositions = FindWallsInDirections(floorPos, Direction2D.cardinalDirectionsList);
        foreach (var position in basicWallPositions)
        {

            tilemapVisualizer.PaintSingleBasicWall(position);

        }
        
    }
    
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {

        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            foreach (var direction in directionList)
            {
                
                var neighbourPos = pos + direction;
                if (floorPositions.Contains(neighbourPos) == false)
                    wallPositions.Add(neighbourPos);
                
            }
            
        }

        return wallPositions;
    }
    
}
