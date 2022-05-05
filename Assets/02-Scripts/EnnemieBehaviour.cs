using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private float speed;
    private void Awake()
    {
        print("stare");
        point = transform.GetChild(0).gameObject;
        body = transform.GetChild(1).gameObject;
        Map.currentRoom.mapp.Add(point.transform.position);
       
    }
    private void Update()
    {
        body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
        if (PlayerMovement.Turn)
        {
            print("omggg");
            var temp = point.transform.position + CheapPathFinding();
            if (!Map.currentRoom.mapp.Contains(temp))
            {
                Map.currentRoom.mapp.Remove(point.transform.position);
                point.transform.position = temp;
                Map.currentRoom.mapp.Add(point.transform.position);
            }
           
        }
    }
    private Vector3 CheapPathFinding()
    {
        Vector2 his = GameObject.Find("point").transform.position;
        Vector2 mine = point.transform.position;
        print(his + " : " + mine);
        if (Random.Range(0, 2) == 0)
        {
            if (his.x < mine.x)
            {
                return new Vector3(-1, 0);
            }
            if (his.y < mine.y)
            {
                return new Vector3(0, -1);
            }
        }
        else
        {
            if (his.y < mine.y)
            {
                return new Vector3(0, -1);
            }
            if (his.x < mine.x)
            {
                return new Vector3(-1, 0);
            }
           
        }
        if (Random.Range(0, 2) == 0)
        {
            if (his.x > mine.x)
            {
                return new Vector3(1, 0);
            }
            if (his.y > mine.y)
            {
                return new Vector3(0, 1);
            }
        }
        else
        {
            if (his.y > mine.y)
            {
                return new Vector3(0, 1);
            }
            if (his.x > mine.x)
            {
                return new Vector3(1, 0);
            }
           
        }
        return new Vector3(0, 0);



    }
}
