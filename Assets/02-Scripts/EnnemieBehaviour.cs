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

    public Ennemie ennemie;
    public WeaponBehaviour arm;
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
            Map.currentRoom.map[conv(point.transform.position)].block = false;
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
            var hit = false;
            for (int i = 0; i< Map.currentRoom.map[vect].attacks.Count; i++)
            {
                if  (Map.currentRoom.map[vect].attacks[i].whom == 0)
                {
                    var attack = Map.currentRoom.map[vect].attacks[i];
                    
                    SetDammages(attack.initialPosition, attack.weapon.Dammage.x);
                    hit = true;

                }
            }
           
            if(!hit)
            {
                //get the CheapPathFinding and if nothing blocks it it go there, if the player block it it will dammage him
                var cheap = CheapPathFinding();
                var temp = conv((Vector2)point.transform.position + cheap);
                print(Map.currentRoom.map.ContainsKey(temp));
                if (!Map.currentRoom.map[temp].block)
                {

                   
                    Map.currentRoom.map[conv(point.transform.position)].block = false;
                    point.transform.position = temp;
                    Map.currentRoom.map[conv(point.transform.position)].block = true;
                   
                }
                else
                {
                    //arm.setAttack(0);
                }
            }
           

           

        }
        wait = false;
    }
    Vector2 conv(Vector2 bas)
    {
        return new Vector2Int((int)bas.x, (int)bas.y);
    }
    //set the damges
    public void SetDammages(Vector2 recule, float damage)
    {
        healthbar.SetActive(true);
        currentLife-=damage;
        wait = true;
        if(!Map.currentRoom.map[conv((Vector2)point.transform.position + recule)].block)
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
            Map.currentRoom.map[(Vector2)point.transform.position].block = false;
            point.transform.position += vect;
            Map.currentRoom.map[(Vector2)point.transform.position].block = true;
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
