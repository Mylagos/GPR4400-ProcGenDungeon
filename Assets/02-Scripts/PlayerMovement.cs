using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    //input
    PlayerInput _input;

    //attribute
    [SerializeField]
    private int speed;
    [SerializeField]
    private float life;
    private static float currentlife;

    public WeaponBehaviour arm;
    //displacement elements
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private Vector2Int ymax;
    [SerializeField]
    private Vector2Int xmax;

    
    //personnal temp variables
    int direction = 0;
    int[] directions = {0, 90, 180, 270};
    bool OnDoor;
    Vector2 direct;

    //helpfull variables for other scripts
    public static GameObject player;
    public static GameObject points;
    public static Vector2 positiion;
    public static int Turn = 0;
    public static bool IsMooving;


    private void Start()
    {
        arm = GetComponent<WeaponBehaviour>();
        //set the static variable so that other script can access it
       
        currentlife = life;
        Turn = 0;

        //set the input
        _input = GetComponent<PlayerInput>();

        //add the position of the player as a collision on the map
        Map.currentRoom.map[transform.position].block = true;
        player = gameObject;
        points = point;
    }
    //move take as input a vector for exemple vector(0,1) (move to the top)
    //and take a direction, if the player is not oriented to this direction,
    //the player is not moving but simply rotating.
    //If force is true the direction dosnt matter
    private void Move(Vector3 deplacement, int dir, bool force = false)
    {
        if (dir == direction || force)
        {
            if (transform.position == point.transform.position)
            {
                Vector3 temp = point.transform.position + deplacement;
                if(temp.y > ymax.x && temp.y < ymax.y && temp.x > xmax.x && temp.x < xmax.y){
                    if (!Map.currentRoom.map[point.transform.position + deplacement].block)
                    {
                        OnDoor = false;
                        print(point.transform.position);
                        Map.currentRoom.map[point.transform.position].block = false;
                        point.transform.position += deplacement;
                        print(transform.position);
                        Map.currentRoom.map[point.transform.position].block = true;
                        if (!force)
                        {
                            Turn = 1;
                        }
                    }

                    //if (!Map.currentRoom.mapp.Contains(point.transform.position + deplacement))
                    //{
                    //    OnDoor = false;
                    //    Map.currentRoom.mapp.Remove(transform.position);
                    //    point.transform.position += deplacement;
                    //    Map.currentRoom.mapp.Add(point.transform.position);
                    //}
                }
            }
        }
        else
        {
            Turn = 1;
            direction = dir;
            transform.eulerAngles = new Vector3(0, 0, directions[dir]);
        }
    }
    //dammages is the direction were the player will be push after taking the damages
    //ammount is the amount of damages taken

    public void SetDammages(Vector2 dammages,float ammount)
    {
        Move(dammages,0,true);
        currentlife -= ammount;
        if (currentlife < 0)
        {
            print("sie");
        }
    }

    private void Update()
    {
        if (Turn>0)
        {
            for (int i = 0; i < Map.currentRoom.map[point.transform.position].attacks.Count; i++)
            {
                var attack = Map.currentRoom.map[transform.position].attacks[i];
                if (attack.whom == 0)
                {
                    SetDammages(attack.initialPosition, attack.weapon.Dammage.x);
                }
            }
        }
        positiion = transform.position;

        //IsMooving is false if the player is not moving vice versa
        if (transform.position == point.transform.position)
        {
            IsMooving = false;
        }
        else
        {
            IsMooving = true;
        }


        List<Vector3> newpos = new List<Vector3>() { new Vector3(0, -4) , new Vector3(0, 4), new Vector3(-7, 0), new Vector3(7, 0) };
        List<Vector2Int> newdir = new List<Vector2Int>() { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };
        //Check if the player is on a door, it then set the bool ondoor true so it doesn't do it each frame, ondoor is false when the player mooves
        for (int i = 0; i < 4 && OnDoor==false; i++)
        {
            if (transform.position == Map.doors[i].transform.position && Map.doors[i].activeInHierarchy)
            {
                GameObject.Find("Main Camera").transform.GetComponent<Map>().move(newdir[i], newpos[i]);
                //transform.position = newpos[i];
                //point.transform.position = newpos[i];
                OnDoor = true;
                break;
            }
        }
        List<string> InputMouvementsNames = new List<string>() {"up", "down", "right","left" };
        int[] tem = { 2, 0, 1,3 };
        //A turn passes
        if (Turn > 0)
        {
            Turn -= 1;
        }
        //Move if input triggered
        for (int i = 0; i<4 && IsMooving == false;i+=1)
        {
            if (_input.actions[InputMouvementsNames[i]].triggered)
            {
                direct = newdir[i];
                Move((Vector2) newdir[i], tem[i]);
                break;
            }
        }
        //God Mod, instant change room with the arrows
        for (int i = 0; i < 4 && IsMooving == false; i += 1)
        {
            if (_input.actions["god"+i.ToString()].triggered)
            {
                GameObject.Find("Main Camera").transform.GetComponent<Map>().move(newdir[i],newpos[i]);
                break;
            }
        }
        //Attack the ennemies in front of the player
        if (_input.actions["Attack"].triggered && !IsMooving)
        {
            arm.setAttack(tem[direction]);
            Turn = 1;
        }
        transform.position = Vector2.MoveTowards(transform.position, point.transform.position, speed *Time.deltaTime);
    }
}
