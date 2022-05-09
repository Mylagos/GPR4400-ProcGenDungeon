using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public int Rooms;
    public Vector2Int size;
    public List<Ennemie> ennemies;
    public List<RoomsBackgrounds> backgrounds;
    public List<Vector2Int> RoomTypes;
    public Vector2Int ennemiesAmmount;

}
