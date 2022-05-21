using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder : MonoBehaviour
{
    [SerializeField] private int _nbIterationsMax;
    [SerializeField]
    [Range(0.1f, 2)] private float _heuristicFactor = 1f;

    public void GeneratePath(List<Vector2Int> map, Vector2Int start, Vector2Int goal)
    {

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
