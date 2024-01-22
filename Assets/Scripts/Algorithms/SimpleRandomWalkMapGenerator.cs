using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : MonoBehaviour
{

    [SerializeField]
    protected Vector2Int startPos = Vector2Int.zero;
    
    [SerializeField]
    private int iterations = 10;
    
    [SerializeField]
    protected int walkLength = 10;
    
    [SerializeField]
    public bool startRandomlyEachIteration = true;

    public void RunProceduralGeneration()
    {

        HashSet<Vector2Int> floorPos = RunRandomWalk();
        foreach (var pos in floorPos)
        {
            
            Debug.Log(pos);
            
        }

    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {

        var currentPos = startPos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for (int i = 0; i < iterations; i++)
        {
            
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, walkLength);
            floorPos.UnionWith(path);
            if (startRandomlyEachIteration)
                currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
            
        }

        return floorPos;

    }

}
