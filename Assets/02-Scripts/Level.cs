using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public int Rooms;
    public Vector2 Size;
    public List<RoomsBackgrounds> backgrounds;
    public List<Vector2> RoomTypes;

}
