using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeaponBehaviour : MonoBehaviour
{
    public Weapon me;
    int pos;
    int attack = 0;
    List<List<Vector2Int>>[] posibilities = new List<List<Vector2Int>>[4];
    List<Vector2Int> used = new List<Vector2Int>();
    List<GameObject> anim = new List<GameObject>();
    List<Attack> currentAttacks = new List<Attack>();
    public GameObject thisGameobject;
    //0 == player, 1== ennemies, 2 == tt le monde
    public int WhoDammage;
    public void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            posibilities[i] = new List<List<Vector2Int>>();
            for (int k = 0; k < me.frames; k++)
            {
                posibilities[i].Add(new List<Vector2Int>());
            }
        }
        for (int n = me.mapDammage.cellBounds.xMin; n < me.mapDammage.cellBounds.xMax; n++)
        {
            for (int p = me.mapDammage.cellBounds.yMin; p < me.mapDammage.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)me.mapDammage.transform.position.y));
                Vector3 place = me.mapDammage.CellToWorld(localPlace); 
                if (me.mapDammage.HasTile(localPlace))
                {
                    var name = me.mapDammage.GetTile(localPlace).name;
                    if (me.mapDammage.GetTile(localPlace).name != "CENTER")
                    {
                        var number = int.Parse(name);
                        //droite
                        posibilities[0][number].Add((Vector2Int)localPlace);
                        //gauche
                        posibilities[3][number].Add(new Vector2Int(-localPlace.x, localPlace.y));
                        //haut
                        posibilities[1][number].Add(new Vector2Int(localPlace.y, localPlace.x));
                        //bas
                        posibilities[2][number].Add(new Vector2Int(localPlace.y, -localPlace.x));

                    }
                }
            }
        }

    }
    public void setAttack(int pos)
    {
        print(currentAttacks.Count);
        currentAttacks.Add(new Attack(posibilities[pos], transform.position,me,WhoDammage));
    }
    public void Update()
    {
        if (PlayerMovement.Turn > 0)
        {
            for (int i = 0; i< currentAttacks.Count; i++)
            {
                if (currentAttacks[i].update())
                {
                    currentAttacks[i].destroyAt();
                    currentAttacks.RemoveAt(i);
                    i--;
                }
            }
            
            
        }

    }

   
}
