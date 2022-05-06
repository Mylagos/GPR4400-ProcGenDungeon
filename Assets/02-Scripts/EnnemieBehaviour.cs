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
    [SerializeField]
    private float life;
    bool wait;
    private void Awake()
    {
        print("stare");
        point = transform.GetChild(0).gameObject;
        body = transform.GetChild(1).gameObject;
        Map.currentRoom.mapp.Add(point.transform.position);
       
    }
    private void Update()
    {
        if (life < 0)
        {
            Map.currentRoom.mapp.Remove(point.transform.position);
            Destroy(gameObject);
        }
        body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
        if (PlayerMovement.Turn > 0 && wait == false )
        {
            var temp = point.transform.position + CheapPathFinding();
            if (!Map.currentRoom.mapp.Contains(temp))
            {
                Map.currentRoom.mapp.Remove(point.transform.position);
                point.transform.position = temp;
                Map.currentRoom.mapp.Add(point.transform.position);
            }
           
        }
        wait = false;
    }
    public bool damage(Vector3 recule,float damage)
    {
        wait = true;
        if (!Map.currentRoom.mapp.Contains(point.transform.position + recule))
        {
            Map.currentRoom.mapp.Remove(point.transform.position);
            point.transform.position += recule;
            Map.currentRoom.mapp.Add(point.transform.position);
        }
        life -= damage;
        if (life < 0)
        {
            return true;
        }
        return false;
    }
    private Vector3 CheapPathFinding()
    {
        Vector2 his = GameObject.Find("point").transform.position;
        Vector2 mine = point.transform.position;
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
