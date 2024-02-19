using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;

    [SerializeField] private bool drunkardsWalkRooms = false;
    
    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }
    
    private void CreateRooms()
    {
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        floor = CreateSimpleRooms(roomList);
        
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        
        foreach (var room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

    }
    
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        
        roomCenters.Remove(currentRoomCenter);
        
        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }
    
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = currentRoomCenter;

        corridor.Add(pos);
        
        while (pos.y != destination.y)
        {
            if(destination.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if(destination.y < pos.y)
            {
                pos += Vector2Int.down;
            }
            corridor.Add(pos);
        }
        while(pos.x != destination.x)
        {
            if(destination.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if(destination.x < pos.x)
            {
                pos += Vector2Int.left;
            }
            corridor.Add(pos);
        }

        return corridor;
    }
    
    /*private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = currentRoomCenter;

        // Add initial position with width
        AddWidthToCorridor(corridor, pos, 3);
    
        while (pos.y != destination.y)
        {
            if(destination.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if(destination.y < pos.y)
            {
                pos += Vector2Int.down;
            }
            AddWidthToCorridor(corridor, pos, 3);
        }
        while(pos.x != destination.x)
        {
            if(destination.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if(destination.x < pos.x)
            {
                pos += Vector2Int.left;
            }
            AddWidthToCorridor(corridor, pos, 3);
        }

        return corridor;
    }

    private void AddWidthToCorridor(HashSet<Vector2Int> corridor, Vector2Int position, int width)
    {
        int halfWidth = width / 2;
        for (int i = -halfWidth; i <= halfWidth; i++)
        {
            if (width % 2 == 0 && i == 0) continue; // Skip the middle line for even widths to avoid uneven corridors
            corridor.Add(new Vector2Int(position.x + i, position.y));
            corridor.Add(new Vector2Int(position.x, position.y + i));
        }
    }
    */

    
    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        
        foreach (var pos in roomCenters)
        {
            float currentDistance = Vector2.Distance(pos, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = pos;
            }
        }

        return closest;
    }
    
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }

        return floor;
    }
    
}
