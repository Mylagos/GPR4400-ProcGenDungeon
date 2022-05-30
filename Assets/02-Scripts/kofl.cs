using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kofl : MonoBehaviour
{
    public static Vector2 vectorInt(Vector2 bas)
    {
        //print("before conv : " + bas + "after conv : " + new Vector2((int)bas.x, (int)bas.y));
        return new Vector2((int)bas.x, (int)bas.y);
    }
    public static Vector2Int vector2Int(Vector3 bas)
    {
        //print("before conv : " + bas + "after conv : " + new Vector2((int)bas.x, (int)bas.y));
        return new Vector2Int((int)bas.x, (int)bas.y);
    }

    public static Vector3 vector2Int2vect3(Vector2Int bas)
    {
        //print("before conv : " + bas + "after conv : " + new Vector2((int)bas.x, (int)bas.y));
        return new Vector3((int)bas.x, (int)bas.y);
    }
}
