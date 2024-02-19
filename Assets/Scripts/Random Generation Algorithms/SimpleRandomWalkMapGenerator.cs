using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{
    
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParameters;
    
    protected override void RunProceduralGeneration()
    {

        HashSet<Vector2Int> floorPos = RunRandomWalk(randomWalkParameters, startPos);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);

    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData parameter, Vector2Int position)
    {

        var currentPos = position;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for (int i = 0; i < parameter.iterations; i++)
        {
            
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, parameter.walkLength);
            floorPos.UnionWith(path);
            if (parameter.startRandomlyEachIteration)
                currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
            
        }

        return floorPos;

    }

}
