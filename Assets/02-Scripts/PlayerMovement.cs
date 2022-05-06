using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput _input;
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private int speed;
    [SerializeField]
    private Vector2Int ymax;
    [SerializeField]
    private Vector2Int xmax;
    [SerializeField]
    private float dammages;
    int direction = 0;
    int[] directions = {0, 90, 180, 270};
    bool OnDoor;
    Vector2 direct;
    
    public static int Turn = 0;

    public static bool IsMooving;
    private void Start()
    {
        Turn = 0;
        _input = GetComponent<PlayerInput>();
        Map.currentRoom.mapp.Add(transform.position);
      
    }
    
    private void Move(Vector3 deplacement,int dir)
    {
        if (dir == direction)
        {
            if (transform.position == point.transform.position)
            {
                Vector3 temp = point.transform.position + deplacement;
                if(temp.y > ymax.x && temp.y < ymax.y && temp.x > xmax.x && temp.x < xmax.y){
                    if (!Map.currentRoom.mapp.Contains(point.transform.position + deplacement))
                    {
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
    private void Update()
    {
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
        //Check if the player is on a door, it then set the bool ondoor true so it doesn't do it each frame, ondoor is flase when the player mooves
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
        if (Turn > 0)
        {
            Turn -= 1;
        }
        
        for (int i = 0; i<4 && IsMooving == false;i+=1)
        {
            if (_input.actions[InputMouvementsNames[i]].triggered)
            {
                direct = newdir[i];
                Move((Vector2) newdir[i], tem[i]);
                OnDoor = false;
                Turn =1;
                break;
            }
        }

        for (int i = 0; i < 4 && IsMooving == false; i += 1)
        {
            if (_input.actions["god"+i.ToString()].triggered)
            {
                Map.move(newdir[i]);
                break;
            }
        }
        if (_input.actions["Attack"].triggered)
        {
            print("attacked");
            for (int i = 0; i< Map.currentRoom.ennemies.Count; i++)
            {
                print((transform.position + (Vector3)direct) + "  : " + Map.currentRoom.ennemies[i].transform.position);
                if (Map.currentRoom.ennemies[i].transform.GetChild(0).transform.position == transform.position + (Vector3)direct)
                {
                    print("hit");
                    if(Map.currentRoom.ennemies[i].GetComponent<EnnemieBehaviour>().damage(direct, dammages))
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
