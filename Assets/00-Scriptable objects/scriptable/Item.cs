using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public int stats;
    public int increaseAmmount;
    public Sprite Sprite;

}
