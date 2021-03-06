using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ennemie", menuName = "ScriptableObjects/Ennemie")]
public class Ennemie : ScriptableObject
{
    public GameObject me;
    public int range;
    public int rangewide;
    public Sprite sprite;
    public Animator animation;
    public Weapon weapon;
    public Vector2Int money;
    public Vector2Int size;
    public int moves;
}
