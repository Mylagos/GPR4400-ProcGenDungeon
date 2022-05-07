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
    private float dammages;
    [SerializeField]
    private float life;
    private static float currentlife;

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
    public static Vector2 positiion;
    public static int Turn = 0;
    public static bool IsMooving;


    private void Start()
    {
        //set the static variable so that other script can access it
        player = gameObject;
        currentlife = life;
        Turn = 0;

        //set the input
        _input = GetComponent<PlayerInput>();

        //add the position of the player as a collision on the map
        Map.currentRoom.mapp.Add(transform.position);
      
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
                    if (!Map.currentRoom.mapp.Contains(point.transform.position + deplacement))
                    {
                        print("auhsdiauh");
                        OnDoor = false;
                        Map.currentRoom.mapp.Remove(transform.position);
                        point.transform.position += deplacement;
                        Map.currentRoom.mapp.Add(point.transform.position);
                    }
                }
            }
        }
        else
        {
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
                Map.move(newdir[i]);
                transform.position = newpos[i];
                point.transform.position = newpos[i];
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
                
                Turn =1;
                break;
            }
        }
        //God Mod, instant change room with the arrows
        for (int i = 0; i < 4 && IsMooving == false; i += 1)
        {
            if (_input.actions["god"+i.ToString()].triggered)
            {
                Map.move(newdir[i]);
                break;
            }
        }
        //Attack the ennemies in front of the player
        if (_input.actions["Attack"].triggered)
        {
            for (int i = 0; i< Map.currentRoom.ennemies.Count; i++)
            {
                print((transform.position + (Vector3)direct) + "  : " + Map.currentRoom.ennemies[i].transform.position);
                if (Map.currentRoom.ennemies[i].transform.GetChild(0).transform.position == transform.position + (Vector3)direct)
                {
                    if(Map.currentRoom.ennemies[i].GetComponent<EnnemieBehaviour>().SetDammages(direct, dammages))
                    {
                        Map.currentRoom.ennemies.RemoveAt(i);
                        break;
                    }
                }
            }
            Turn = 1;
        }
        transform.position = Vector2.MoveTowards(transform.position, point.transform.position, speed *Time.deltaTime);
    }
}
