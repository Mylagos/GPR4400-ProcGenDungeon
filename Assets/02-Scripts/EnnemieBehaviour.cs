using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieBehaviour : MonoBehaviour
{

    [SerializeField]
    private GameObject point;
    [SerializeField]
    private GameObject body;

    //stats
    [SerializeField]
    private float speed;
    [SerializeField]
    public float life;
    [SerializeField]
    public float damage;

    public float currentLife;
    bool wait;
    public GameObject healthbar;
    private void Awake()
    {
        currentLife = life;
        point = transform.GetChild(0).gameObject;
        body = transform.GetChild(1).gameObject;
        Map.currentRoom.mapp.Add(point.transform.position);


        // - new Vector3(0,1)to the ennemies;
        healthbar = Instantiate(GameObject.Find("healthbar"), gameObject.transform.GetChild(1).transform.position - new Vector3(0, 1), Quaternion.identity, GameObject.Find("Canvas").transform);
        healthbar.GetComponent<Healthbar>().Father = gameObject;
        healthbar.GetComponent<Healthbar>().child = true;
        healthbar.SetActive(false);

    }
    private void Update()
    {
        //die
        if (currentLife < 0)
        {
            Destroy(gameObject);
            Destroy(healthbar);
            Map.currentRoom.mapp.Remove(point.transform.position);
        }
        //follow the point
        body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
        //if the ennemies can make a move
        if (PlayerMovement.Turn > 0 && wait == false)
        {
            //get the CheapPathFinding and if nothing blocks it it go there, if the player block it it will dammage him
            var cheap = CheapPathFinding();
            var temp = (Vector2)point.transform.position + cheap;
            if (!Map.currentRoom.mapp.Contains(temp))
            {
                Map.currentRoom.mapp.Remove(point.transform.position);
                point.transform.position = temp;
                Map.currentRoom.mapp.Add(point.transform.position);
            }
            else if (temp == PlayerMovement.positiion)
            {
                PlayerMovement.player.GetComponent<PlayerMovement>().SetDammages(cheap, damage);
            }

        }
        wait = false;
    }
    //set the damges
    public bool SetDammages(Vector3 recule, float damage)
    {
        healthbar.SetActive(true);
        currentLife-=damage;
        wait = true;
        if(!Map.currentRoom.mapp.Contains(point.transform.position + recule))
        {
            Map.currentRoom.mapp.Remove(point.transform.position);
            point.transform.position += recule;
            Map.currentRoom.mapp.Add(point.transform.position);
        }
        if (currentLife < 0)
        {
            return true;
        }
        return false;
    }
    //return a Cheap as fock Path Finding
    //a wall breaks it
    private Vector2 CheapPathFinding()
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
