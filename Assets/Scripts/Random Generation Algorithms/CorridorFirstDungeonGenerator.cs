using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    
    [SerializeField]
    private float roomPercent = 0.8f;
    
    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
        
    }

    private void CorridorFirstDungeonGeneration()
    {
        
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>();
        
        List<List<Vector2Int>> corridors = CreateCorridors(floorPos, potentialRoomPos);
        
        HashSet<Vector2Int> roomPos = CreateRooms(potentialRoomPos);
        
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPos);
        
        createRoomsAtDeadEnds(deadEnds, roomPos);
        
        floorPos.UnionWith(roomPos);
        
        for(int i = 0; i < corridors.Count; i++)
        {
            /*corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);*/
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPos.UnionWith(corridors[i]);
        }
        
        tilemapVisualizer.PaintFloorTiles(floorPos);
        
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);
        
    }
    
    private void createRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        
        foreach (var pos in deadEnds)
        {
            
            if(roomFloors.Contains(pos) == false) 
            {
                var room = RunRandomWalk(randomWalkParameters, pos);
                roomFloors.UnionWith(room);
            }
            
        }
        
    }
    
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPos)
    {
        
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        
        foreach (var pos in floorPos)
        {
            
            int neighboursCount = 0;
            
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                
                if (floorPos.Contains(pos + direction))
                {
                    neighboursCount++;
                }
                
            }
            
            if (neighboursCount == 1)
            {
                deadEnds.Add(pos);
            }
            
        }
        
        return deadEnds;
        
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPos.Count * roomPercent);
        
        List<Vector2Int> roomsToCreate = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPos in roomsToCreate)
        {
            
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPos);
            
            roomPositions.UnionWith(roomFloor);
            
        }

        return roomPositions;

    }
    
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomPos)
    {
        
        var currentPos = startPos;
        
        potentialRoomPos.Add(currentPos);
        
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        
        for (int i = 0; i < corridorCount; i++)
        {
            
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPos, corridorLength);
            corridors.Add(corridor);
            currentPos = corridor[corridor.Count - 1];
            
            potentialRoomPos.Add(currentPos);
            
            floorPos.UnionWith(corridor);
            
        }
        
        return corridors;
        
    }
    
    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;
        
        for (int i = 1; i < corridor.Count; i++)
        {

            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if(previousDirection != Vector2Int.zero && directionFromCell != previousDirection)
            {
               
                //handle corner
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                        
                    }
                }
                
                previousDirection = directionFromCell;
                
            }
            else
            {
                
                //Add a single cell in the direction + 90 degrees
                Vector2Int newCorridorTileOffset = GetDirection90Degrees(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
                previousDirection = directionFromCell;
                
            }

        }
        
        return newCorridor;
        
    }

    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        
        List<Vector2Int> newCorridor = new List<Vector2Int>();

        for (int i = 1; i < corridor.Count; i++)
        {
            for(int x = -1; x < 2; x++)
            {
                for(int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridor;

    }
    
    private Vector2Int GetDirection90Degrees(Vector2Int direction)
    {
        
        if (direction == Vector2Int.up)
        {
            return Vector2Int.right;
        }
        if (direction == Vector2Int.right)
        {
            return Vector2Int.down;
        }
        if (direction == Vector2Int.down)
        {
            return Vector2Int.left;
        }
        if (direction == Vector2Int.left)
        {
            return Vector2Int.up;
        }
        
        return Vector2Int.zero;
        
    }
    
    
}
