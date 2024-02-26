using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DrunkardsWalkMapGenerator : AbstractDungeonGenerator
{
    
    [SerializeField]
    protected DrunkardsWalkData drunkardsWalkParameters;
    
    protected override void RunProceduralGeneration()
    {

        HashSet<Vector2Int> floorPos = RunDrunkardsWalk(drunkardsWalkParameters, startPos);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);

    }

    protected HashSet<Vector2Int> RunDrunkardsWalk(DrunkardsWalkData parameter, Vector2Int position)
    {

        var currentPos = position;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for (int i = 0; i < parameter.iterations; i++)
        {
            
            var path = ProceduralGenerationAlgorithms.DrunkardsWalk(currentPos, parameter.walkLength);
            floorPos.UnionWith(path);
            if (parameter.startRandomlyEachIteration)
                currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
            
        }

        return floorPos;

    }

}
