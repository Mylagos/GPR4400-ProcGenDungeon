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
    int[] directions = { 0, 90, 180, 270 };
    public float currentLife;
    bool wait;
    public GameObject healthbar;
    public Ennemie ennemie;
    public WeaponBehaviour arm;
    public int direction = 0;
    public bool isMoving = false;
    private void Awake()
    {
        currentLife = life;
        point = transform.GetChild(0).gameObject;
        body = transform.GetChild(1).gameObject;
        arm = transform.GetChild(1).GetComponent<WeaponBehaviour>();

        // - new Vector3(0,1)to the ennemies;
        healthbar = Instantiate(GameObject.Find("healthbar"), gameObject.transform.GetChild(1).transform.position - new Vector3(0, 1), Quaternion.identity, GameObject.Find("Canvas").transform);
        healthbar.GetComponent<Healthbar>().Father = gameObject;
        healthbar.GetComponent<Healthbar>().child = true;
        healthbar.SetActive(false);

    }
    public Vector2 Closest()
    {
        
        List<Vector2> pos = new List<Vector2>();
        var vect = kofl.vectorInt(PlayerMovement.points.transform.position);
        var vect1 = kofl.vectorInt(point.transform.position);
        for ( int i  = -ennemie.rangewide/2; i <= ennemie.rangewide/2;i++)
        {
            pos.Add(vect + new Vector2(0,ennemie.range + i));
            pos.Add(vect + new Vector2(0, -ennemie.range + i));
            pos.Add(vect + new Vector2(-ennemie.range + i, 0));
            pos.Add(vect + new Vector2(ennemie.range + i, 0));
        }
        if (pos.Contains(vect1))
        {
            return vect1;
        }
        float min = 9999999;
        var minind = 0;
        for (int i = 0; i < pos.Count; i++)
        {
            var temo = Vector2.Distance(vect1, pos[i]);
            if (temo < min)
            {
                min = temo;
                minind = i;
            }
        }
        for (int i = 4; i > 0; i--)
        {
            if (minind % i == 0)
            {
                transform.eulerAngles = new Vector3(0,0, directions[i - 1]);
                direction = i;
            }
        }
        if (Vector2.Distance(vect1, vect) < min)
        {
            return vect1;
        }
       
        return pos[minind];
    }
    private void Update()
    {
        if (body.transform.position ==point.transform.position)
        {
            isMoving = false;
        }else
        {
            isMoving = true;
        }
        //die
        if (currentLife < 0)
        {
            Map.currentRoom.map[kofl.vectorInt(point.transform.position)].block = false;
            Map.currentRoom.ennemies.Remove(gameObject);
            Destroy(gameObject);
            Destroy(healthbar);
            
        }
        //if the ennemies can make a move
        wait = false;
    }
   
    //set the damges
    public void SetDammages(Vector2 recule, float damage)
    {
        healthbar.SetActive(true);
        currentLife-=damage;
        wait = true;
        if(!Map.currentRoom.map[kofl.vectorInt((Vector2)point.transform.position + recule)].block)
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
            Map.currentRoom.map[kofl.vectorInt((Vector2)point.transform.position)].block = false;
            point.transform.position += vect;
            Map.currentRoom.map[kofl.vectorInt((Vector2)point.transform.position)].block = true;
        }

    }
    //return a Cheap as fock Path Finding
    public IEnumerator move()
    {
        print("hi");
        yield return new WaitForSeconds(1.2f);
        print("his");
        if (wait == false)
        {
            var vect = kofl.vectorInt(point.transform.position);
            var hit = false;
            for (int i = 0; i < Map.currentRoom.map[vect].attacks.Count; i++)
            {
                if (Map.currentRoom.map[vect].attacks[i].whom == 0)
                {
                    var attack = Map.currentRoom.map[vect].attacks[i];

                    SetDammages(attack.initialPosition, attack.weapon.Dammage.x);
                    hit = true;

                }
            }
            if (!hit)
            {
                if (!isMoving)
                {
                    //return le point ou aller a ameliorer
                    var closest = Closest();
                    print(closest + " : " + vect);
                    //if (closest != vect)
                    //{
                    //var past = pasta.Bestpasta(closest, body.transform.position);
                    //TO DO : var past = astart(PlayerMovement.points.transform.position, body.transform.position);
                    var past = CheapPathFinding();
                    if (!Map.currentRoom.map[kofl.vectorInt(point.transform.position + (Vector3)past)].block)
                    {
                        Map.currentRoom.map[kofl.vectorInt(point.transform.position)].block = false;
                        point.transform.position += (Vector3)past;
                        Map.currentRoom.map[kofl.vectorInt(point.transform.position)].block = true;
                    }
                }
                else
                {
                    print("shoot");
                    //arm.setAttack(direction);

                    //get the CheapPathFinding and if nothing blocks it it go there, if the player block it it will dammage him


                }

            }

            while (body.transform.position!=point.transform.position)
            {
                body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            
        }
        PlayerMovement.step_by_step = true;
    }
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
