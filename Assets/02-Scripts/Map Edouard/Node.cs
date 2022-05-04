using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private Node _parentNode;
    private List<Node> _childList;
    public Vector2Int _mapPosition;
    //private bool _isLeaf = false;
    private int _seed;

    [System.Serializable]
    public struct ChancesToSpawn
    {
        public string name;
        public int RoomX0;
        public int RoomX1;
        public int RoomX2;
        public int RoomX3;
        public int RoomX4;
    }

    public List<ChancesToSpawn> chancesToSpawn;



    public Node(Vector2Int position, Node parent = null)
    {
        _mapPosition = position;
        _parentNode = parent;
    }


}
