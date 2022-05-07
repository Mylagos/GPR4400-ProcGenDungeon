using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RoomsBackgrounds", menuName = "ScriptableObjects/RoomsBackgrounds")]
public class RoomsBackgrounds : ScriptableObject
{
    public GameObject gameObject;
    public Vector2Int[] doorLocation;
    public Vector2 ChessLocation;

}
