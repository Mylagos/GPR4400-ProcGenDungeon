using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemieBehaviour : MonoBehaviour
{

    [SerializeField]
    private GameObject point;
    [SerializeField]
    private GameObject body;
    Animator anime;
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
    public int direction = 0;
    public bool isMoving = false;
    public float waittime;
    public bool animeAstar;
    List<GameObject> pathobject = new List<GameObject>();
    List<Vector2> directions = new List<Vector2>() { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0),new Vector2(-1, 0) };
    private void Awake()
    {
        
        currentLife = life;
        point = transform.GetChild(0).gameObject;
        body = transform.GetChild(1).gameObject;
        arm = transform.GetChild(1).GetComponent<WeaponBehaviour>();
        anime = body.transform.GetChild(0).GetComponent<Animator>();
        // - new Vector3(0,1)to the ennemies;
        healthbar = Instantiate(GameObject.Find("healthbar"), gameObject.transform.GetChild(1).transform.position - new Vector3(0, 1), Quaternion.identity, GameObject.Find("Canvas").transform);
        healthbar.GetComponent<Healthbar>().Father = gameObject;
        healthbar.GetComponent<Healthbar>().child = true;
        healthbar.SetActive(false);

    }
    //public Vector2 Closest()
    //{

    //    List<Vector2> pos = new List<Vector2>();
    //    var vect = VectorHelper.vectorInt(PlayerMovement.points.transform.position);
    //    var vect1 = VectorHelper.vectorInt(point.transform.position);
    //    for (int i = -ennemie.rangewide / 2; i <= ennemie.rangewide / 2; i++)
    //    {
    //        pos.Add(vect + new Vector2(0, ennemie.range + i));
    //        pos.Add(vect + new Vector2(0, -ennemie.range + i));
    //        pos.Add(vect + new Vector2(-ennemie.range + i, 0));
    //        pos.Add(vect + new Vector2(ennemie.range + i, 0));
    //    }
    //    if (pos.Contains(vect1))
    //    {
    //        return vect1;
    //    }
    //    float min = 9999999;
    //    var minind = 0;
    //    for (int i = 0; i < pos.Count; i++)
    //    {
    //        var temo = Vector2.Distance(vect1, pos[i]);
    //        if (temo < min)
    //        {
    //            min = temo;
    //            minind = i;
    //        }
    //    }
    //    for (int i = 4; i > 0; i--)
    //    {
    //        if (minind % i == 0)
    //        {
    //            transform.eulerAngles = new Vector3(0, 0, directions[i - 1]);
    //            direction = i;
    //        }
    //    }
    //    if (Vector2.Distance(vect1, vect) < min)
    //    {
    //        return vect1;
    //    }

    //    return pos[minind];
    //}
    private void Update()
    {
        if (body.transform.position == point.transform.position)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        if (PlayerMovement.Turn>0)
        {

            var vect = VectorHelper.vectorInt(point.transform.position);
            for (int i = 0; i < Map.currentRoom.map[vect].attacks.Count; i++)
            {
                if (Map.currentRoom.map[vect].attacks[i].whom == 0)
                {
                    var attack = Map.currentRoom.map[vect].attacks[i];

                    StartCoroutine(SetDammages(attack.initialPosition, attack.weapon.Dammage.x));


                }
            }
        }
        //die
        if (currentLife < 0)
        {
            Map.currentRoom.map[VectorHelper.vectorInt(point.transform.position)].block = false;
            Map.currentRoom.ennemies.Remove(gameObject);
            Destroy(gameObject);
            Destroy(healthbar);

        }
        //if the ennemies can make a move
        wait = false;
    }

    //set the damges
    IEnumerator SetDammages(Vector2 recule, float damage)
    {
        healthbar.SetActive(true);
        currentLife -= damage;
        wait = true;
        if (!Map.currentRoom.map[VectorHelper.vectorInt((Vector2)point.transform.position + recule)].block)
        {
            var vect = new Vector3();
            if (recule.x == 0)
            {
                vect = new Vector3(0, Mathf.Abs(recule.y) / recule.y);
            }
            else if (recule.y == 0)
            {
                vect = new Vector3(Mathf.Abs(recule.x) / recule.x, 0);
            }
            else
            {
                vect = new Vector3(Mathf.Abs(recule.x) / recule.x, Mathf.Abs(recule.y) / recule.y);
            }
           
            Map.currentRoom.map[VectorHelper.vectorInt((Vector2)point.transform.position)].block = false;
            point.transform.position += vect;
            Map.currentRoom.map[VectorHelper.vectorInt((Vector2)point.transform.position)].block = true;
           
            while (body.transform.position != point.transform.position)
            {
                anime.SetBool("damaged", true);
                body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            anime.SetBool("damaged", false);
        }
        yield return new WaitForEndOfFrame();

    }
    //return a Cheap as fock Path Finding
    public IEnumerator move()
    {
        anime.SetBool("walk", true);
        var path = AStarPathFinder.GeneratePath(Map.currentRoom.AstarMapVector2Int(), VectorHelper.vector2Int(point.transform.position), VectorHelper.vector2Int(PlayerMovement.points.transform.position));
        path.Reverse();
        for (int moveIdx = 0; moveIdx < ennemie.moves; moveIdx++)
        {
            yield return new WaitForSeconds(waittime);
            if (wait == false)
            {

                //if (closest != vect)
                //{
                //var past = pasta.Bestpasta(closest, body.transform.position);
                //TO DO : var past = astart(PlayerMovement.points.transform.position, body.transform.position);

                if (!isMoving)
                {
                    //try to move, if he cant he shoot;
                    try
                    {

                        direction = directions.IndexOf( (Vector2)VectorHelper.vector2Int2vect3(path[moveIdx].Position)- VectorHelper.vectorInt(point.transform.position) );
                 
                        if (direction == 3)
                        {
                            body.transform.GetChild(0).localScale = new Vector3(-1.5f, 1.5f);
                        }
                        else if(direction ==2)
                        {
                            body.transform.GetChild(0).localScale = new Vector3(1.5f, 1.5f);
                        }
                       
                        Map.currentRoom.map[VectorHelper.vectorInt(point.transform.position)].block = false;
                        point.transform.position = VectorHelper.vector2Int2vect3(path[moveIdx].Position);
                        Map.currentRoom.map[VectorHelper.vectorInt(point.transform.position)].block = true;
                        if (animeAstar)
                        {
                            DebugAStar(path);
                        }

                    }
                    catch
                    {
                        try
                        {
                            direction = directions.IndexOf(VectorHelper.vector2Int(PlayerMovement.points.transform.position) - VectorHelper.vectorInt(point.transform.position));
                            arm.setAttack(direction);
                        }
                        catch { }
                       
                    }



                }

                while (body.transform.position != point.transform.position)
                {
                   
                    body.transform.position = Vector2.MoveTowards(body.transform.position, point.transform.position, speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                


            }
        }
        PlayerMovement.step_by_step = true;
        anime.SetBool("walk", false);
    }

    private void DebugAStar(List<AStarNode> path)
    {
        for (int i = 0; i < pathobject.Count; i++)
        {
            Destroy(pathobject[i]);
        }
        pathobject.Clear();
        for (int i = 0; i < path.Count; i++)
        {
            var obj = Instantiate(Map.cube, VectorHelper.vector2Int2vect3(path[i].Position), Quaternion.identity);
            pathobject.Add(obj);
        }
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
