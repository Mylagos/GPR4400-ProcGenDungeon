using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieBehaviour : MonoBehaviour
{
    private GameObject point;
    private GameObject body;
    [SerializeField]
    private float speed;
    private void Awake()
    {
        point = transform.GetChild(0).gameObject;
        body = transform.GetChild(1).gameObject;
    }
    private void Update()
    {
        body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
        if (PlayerMovement.Turn)
        {
            print("omggg");
            point.transform.localPosition += CheapPathFinding();
        }
    }
    private Vector3 CheapPathFinding()
    {
        Vector2 his = GameObject.Find("point").transform.position;
        Vector2 mine = point.transform.position;
        print(his + " : " + mine);
        if (his.x < mine.x)
        {
            return new Vector3(-1, 0);
        }
        if (his.y < mine.y)
        {
            return new Vector3(0, -1);
        }
        if (his.x > mine.x)
        {
            return new Vector3(1, 0);
        }
        if (his.y > mine.y)
        {
            return new Vector3(0, 1);
        }
        return new Vector3(0,0);

    }
}
