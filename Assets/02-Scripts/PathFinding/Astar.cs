using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathFinder : MonoBehaviour
{
    [SerializeField] private static int _nbIterationsMax = 100;
    [SerializeField]
    [Range(0.1f, 2)] private static float _heuristicFactor = 1f;

    private static List<AStarNode> _open = new List<AStarNode>();
    private static List<AStarNode> _closed = new List<AStarNode>();
    private static List<AStarNode> _path = new List<AStarNode>();
    
    private static Vector2Int _start;
    private static Vector2Int _end;
    private static AStarNode _currentNode;
    public List<AStarNode> Open => _open;
    public List<AStarNode> Closed => _closed;
    public static List<AStarNode> Path => _path;
    public AStarNode CurrentNode => _currentNode;
    public int NbIterationsMax { get => _nbIterationsMax; set => _nbIterationsMax = value; }

    public static List<AStarNode> GeneratePath(List<Vector2Int> map, Vector2Int start, Vector2Int end)
    {
        _nbIterationsMax = 100;
        map.Add(start);
        map.Add(end);
        _open = new List<AStarNode>();
        _closed = new List<AStarNode>();
        _path = new List<AStarNode>();

        _start = start;
        _end = end;
        if (!map.Contains(_start))
            Debug.LogWarning("Start position " + _start + " is out of bounds");
        if (!map.Contains(_end))
            Debug.LogWarning("End position " + _end + " is out of bounds");

        _currentNode = new AStarNode(_start, Vector2Int.Distance(_start, _end), 0, null);

        int nbIterations = 0;

        while (_currentNode.Position != _end && nbIterations < _nbIterationsMax)
        {
            foreach (var Neighbour in Neighbourhood.VonNeumann)
            {
                Vector2Int newPos = _currentNode.Position + Neighbour;
                AStarNode newNode = new AStarNode(
                    newPos,
                    Vector2Int.Distance(newPos, _end) * _heuristicFactor,
                    _currentNode.C + 1,
                    _currentNode);

                if (map.Contains(newPos) && !_open.Exists(Neighbour => Neighbour.Position == newPos) && !_closed.Exists(Neighbour => Neighbour.Position == newPos))
                    _open.Add(newNode);
            }
            _currentNode = _open.OrderBy(o => o.S).ElementAt(0);
            _closed.Add(_currentNode);
            _open.Remove(_currentNode);
            nbIterations++;
        }

        AStarNode rollbacknode = new AStarNode(_currentNode);
        while (rollbacknode.Position != _start)
        {
            rollbacknode = rollbacknode.Parent;
            _path.Add(rollbacknode);
        }

        return _path;
    }
}

public class AStarNode
{
    private Vector2Int _nodePos;
    private float _H;
    private float _C;
    private AStarNode _parent;

    public float S { get { return _H + _C; } }

    public float H { get => _H; set => _H = value; }
    public float C { get => _C; set => _C = value; }
    public AStarNode Parent { get => _parent; set => _parent = value; }
    public Vector2Int Position { get => _nodePos; set => _nodePos = value; }


    public AStarNode(Vector2Int pos, float h, float c, AStarNode parent)
    {
        _nodePos = pos;
        _H = h;
        _C = c;
        _parent = parent;
    }

    public AStarNode(AStarNode copy)
    {
        _nodePos = copy.Position;
        _H = copy._H;
        _C = copy._C;
        _parent = copy.Parent;
    }

}
