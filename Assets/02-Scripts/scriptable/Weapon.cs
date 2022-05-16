using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite sprite;
    public int frames;
    public Vector2 Dammage;
    //les endroit attein par lattaque par defaut choisir la droite
    //vector(0,1),vector(0,2)
    public Tilemap mapDammage;
    
}