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
    private Vector2Int xmax
  ;
    int direction = 0;
    int[] directions = {0, 90, 180, 270};
   

    Map map = new Map();
    [SerializeField]
    private GameObject Cub;
    [SerializeField]
    private List<int> Chances;
    [SerializeField]
    private float rate;
    [SerializeField]
    private int Rooms;
    private void Awake()
    {
        map = new Map(Rooms, Cub,Chances,rate);
        _input = GetComponent<PlayerInput>();
    }
    
    private void Move(Vector3 deplacement,int dir)
    {
        if (dir == direction)
        {
            if (transform.position == point.transform.position)
            {
                Vector3 temp = point.transform.position + deplacement;
                if(temp.y > ymax.x && temp.y < ymax.y && temp.x > xmax.x && temp.x < xmax.y){
                    point.transform.position += deplacement;
                }
                

            }
        }
        else
        {
            direction = dir;
            transform.eulerAngles = new Vector3(0, 0, directions[dir]);
            print(directions[dir]);
        }
        
       
    }
    private void Update()
    {
        if (_input.actions["up"].triggered)
        {
            print("jnasnd");
            Move(new Vector3(0, 1, 0),2);
        }
        else if (_input.actions["down"].triggered)
        {
            Move(new Vector3(0, -1, 0),0);
        }
        else if (_input.actions["left"].triggered)
        {
            Move(new Vector3(-1, 0, 0),3);
        }
        else if (_input.actions["right"].triggered)
        {
            Move(new Vector3(1, 0, 0),1);
        }
        transform.position = Vector2.MoveTowards(transform.position, point.transform.position, speed *Time.deltaTime);
       
  
    }

}
