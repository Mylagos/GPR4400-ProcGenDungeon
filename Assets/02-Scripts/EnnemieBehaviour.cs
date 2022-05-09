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
            Map.currentRoom.mapp.Remove(point.transform.position);
            Map.currentRoom.ennemies.Remove(gameObject);
            Destroy(gameObject);
            Destroy(healthbar);
            
        }
        //follow the point
        body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
        
        //if the ennemies can make a move
        if (PlayerMovement.Turn > 0 && wait == false)
        {
            var vect = new Vector2Int((int)point.transform.position.x, (int)point.transform.position.y);
            if (Map.currentRoom.dammages.ContainsKey(vect))
            {
                var attack = Map.currentRoom.dammages[vect];
                if (attack.whom == 0)
                {
                    SetDammages(attack.initialPosition, attack.dammage);
                    
                }
               
            }
            else
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

           

        }
        wait = false;
    }
    //set the damges
    public void SetDammages(Vector2 recule, float damage)
    {
        healthbar.SetActive(true);
        currentLife-=damage;
        wait = true;
        if(!Map.currentRoom.mapp.Contains((Vector2)point.transform.position + recule))
        {
            var vect = new Vector3();
            if (recule.x == 0)
            {
                 vect = new Vector3(0, Mathf.Abs(recule.y) / recule.y);
            }else if (recule.y == 0)
            {
                 vect = new Vector3(Mathf.Abs(recule.x) / recule.x, 0);
            }
            else
            {
                 vect = new Vector3(Mathf.Abs(recule.x) / recule.x, Mathf.Abs(recule.y) / recule.y);
            }
            Map.currentRoom.mapp.Remove(point.transform.position);
            point.transform.position += vect;
            Map.currentRoom.mapp.Add(point.transform.position);
        }

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
