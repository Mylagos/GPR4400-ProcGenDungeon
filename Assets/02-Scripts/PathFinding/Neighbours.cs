using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Neighbourhood
{
    public static Vector2Int Up = Vector2Int.up;
    public static Vector2Int Right = Vector2Int.right;
    public static Vector2Int Down = Vector2Int.down;
    public static Vector2Int Left = Vector2Int.left;
    public static Vector2Int UpRight = new Vector2Int(1, 1);
    public static Vector2Int DownRight = new Vector2Int(1, -1);
    public static Vector2Int UpLeft = new Vector2Int(-1, 1);
    public static Vector2Int DownLeft = new Vector2Int(-1, -1);

    public static List<Vector2Int> VonNeumann = new List<Vector2Int>
    {
        Up, // Full Up
        Right, // Full Right
        Down, // Full down
        Left // Full left

    };

    public static List<Vector2Int> Diagonals = new List<Vector2Int>
    {
        UpRight, // Full Up
        DownRight, // Full Right
        DownLeft, // Full down
        UpLeft
    };

    public static List<Vector2Int> Moore = new List<Vector2Int>
    {
        Up, // Full Up
        UpRight, // Full Up
        Right, // Full Right
        DownRight, // Full Right
        Down, // Full down
        DownLeft, // Full down
        Left, // Full left
        UpLeft
    };

    public static Vector2Int GetRandomVonNeumannNeighbour()
    {
        return VonNeumann[Random.Range(0, VonNeumann.Count)];
    }
    public static Vector2Int GetRandomMooreNeighbour()
    {
        return Moore[Random.Range(0, Moore.Count)];
    }

    public static string GetHash(Vector2Int position, HashSet<Vector2Int> supposedNeighBours,
        List<Vector2Int> neighbourhoodDirections)
    {
        string neighbourhoodHash = "";
        foreach (Vector2Int direction in neighbourhoodDirections)
        {
            var neighbourhoodPosition = position + direction;
            if (supposedNeighBours.Contains(neighbourhoodPosition))
                neighbourhoodHash += "1";
            else
                neighbourhoodHash += "0";
        }

        return neighbourhoodHash;
    }

    public static int getCount(Vector2Int position, HashSet<Vector2Int> supposedNeighbours,
        List<Vector2Int> neighbourhoodDirections)
    {
        int neighboursCount = 0;

        foreach (Vector2Int direction in neighbourhoodDirections)
        {
            var neighbourhoodPosition = position + direction;
            if (supposedNeighbours.Contains(neighbourhoodPosition))
                neighboursCount++;
        }

        return neighboursCount;
    }
}