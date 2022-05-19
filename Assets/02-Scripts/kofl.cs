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
}
