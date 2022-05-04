using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] private int _maxRooms;
    [SerializeField] private int _mapMaxSize;
    [SerializeField] private int _mapMaxGen;
    private int _mapCenter;

    private List<List<Node>> _map = new List<List<Node>>();
    private List<int> _chances = new List<int>();
    public Tree(int maxRooms, GameObject cube, List<int> chances, int mapMaxSize, int mapMaxGen)
    {
        _maxRooms = maxRooms;
        _cube = cube;
        _chances = chances;
        _mapMaxSize = mapMaxSize;
        _mapMaxGen = mapMaxGen;

        for (int i = 0; i <= _mapMaxSize; i++)
        {
            _map.Add(new List<Node>());
            for (int j = 0; j <= _mapMaxSize; j++)
            {
                _map[i].Add(null);
            }
        }

        _mapCenter = (_mapMaxSize / 2);
        _map[_mapCenter][_mapCenter] = new Node(new Vector2Int(_mapCenter, _mapCenter));
        Generate();
    }

    void Generate()
    {
        List<Node> currentGen = new List<Node>() { _map[_mapCenter][_mapCenter] };
        List<Node> nextGen = new List<Node>();

        int generatedNodes = 0;

        for (int currentGenNumb = 0; currentGenNumb <= _mapMaxGen && generatedNodes <= _maxRooms; currentGenNumb++)
        {
            int currentGenPopu = currentGen.Count;
            for (int nextGenPopu = 0; nextGenPopu <= currentGenPopu && generatedNodes <= _maxRooms; nextGenPopu++)
            {
                List<Vector2Int> availablePaths = CheckAvailable(currentGen[0]._mapPosition);
            }
        }
    }

    List<Vector2Int> CheckAvailable(Vector2Int nodePos)
    {
        List<Vector2Int> availablePaths = new List<Vector2Int>();
        Vector2Int[] vectors = { nodePos + new Vector2Int(0, 1), nodePos + new Vector2Int(1, 0), nodePos + new Vector2Int(0, -1), nodePos + new Vector2Int(-1, 0) };
        for (int i = 0; i < 4; i++)
        {
            try
            {
                if (_map[vectors[i].x][vectors[i].y] == null)
                {
                    availablePaths.Add(vectors[i]);
                }
            }
            catch { }

        }
        return availablePaths;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
