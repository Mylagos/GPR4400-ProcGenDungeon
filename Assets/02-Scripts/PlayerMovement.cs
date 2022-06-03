using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{


    //input
    PlayerInput _input;
    Animator anime;
    //attribute
    [SerializeField]
    private int speed;
    [SerializeField]
    private float life;
    [SerializeField]
    private float currentlife;

    public float MaxHealth
    {
        get => life;
    }
    public float CurrentHealth { get => currentlife; }

    public WeaponBehaviour arm;
    //displacement elements
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private Vector2Int ymax;
    [SerializeField]
    private Vector2Int xmax;

    public int moves_ammount;
    public Tilemap GUI;
    public TileBase normal;

    [SerializeField] public int moves;
    [SerializeField] public int attack;
    //personnal temp variables
    int direction = 0;
    int[] directions = { 0, 90, 180, 270 };
    bool OnDoor;

    //helpfull variables for other scripts
    public static GameObject player;
    public static GameObject points;
    public static Vector2 positiion;
    public static int Turn = 0;
    public static bool IsMooving;
    
    public static bool step_by_step;
    bool canmove = true;

    public SpriteRenderer voile;
    //List<Animation> --> haut : 0 couloir,  1 monte; bas : 2 descend, 3 couloir; droite : 4 descend,5 couloir,  6 monte; gauche : 4 descend,5 couloir,  6 monte;
    public List<RoomChangeAnimationData> RoomChangeAnimations;
   
    private void Start()
    {
        anime = GetComponent<Animator>();
        moves = moves_ammount;
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
    IEnumerator Move()
    {
        GUI.ClearAllTiles();
        if (moves == 0)
        {
            canmove = false;
            step_by_step = false;
            foreach (GameObject obj in Map.currentRoom.ennemies)
            {
                step_by_step = false;
                StartCoroutine(obj.GetComponent<EnnemieBehaviour>().move());
                while (step_by_step == false)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            canmove = true;
            moves = moves_ammount;
        }
        DrawPossibleMoves();
    }
    IEnumerator RoomChangeAnimation(Vector2 newpos,Vector2Int newdir)
    {
        GUI.ClearAllTiles();
        moves = moves_ammount;
        canmove = false;
        List<Vector2Int> newdirs = new List<Vector2Int>() { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };

        var temp = Map.map[Map.currentRoom.position.x + newdir.x][Map.currentRoom.position.y + newdir.y];
        var differrence = 0;
        differrence = temp.height - Map.currentRoom.height;
        //List<Animation> --> haut : 0 couloir,  1 monte; bas : 2 descend, 3 couloir; droite : 4 descend,5 couloir,  6 monte; gauche : 4 descend,5 couloir,  6 monte;
        RoomChangeAnimationData anim = null;
        switch (newdir)
        {
            case Vector2Int v when v.Equals(new Vector2Int(0,1)):
                switch (differrence)
                {
                    case 0:
                        print(0);
                        anim = RoomChangeAnimations[0];
                        break;
                    case 1:
                        print(1);
                        anim = RoomChangeAnimations[1];
                        break;
                }
                break;
            case Vector2Int v when v.Equals(new Vector2Int(0, -1)):
                switch (differrence)
                {
                    case -1:
                        print(2);
                        anim = RoomChangeAnimations[2];
                        break;
                    case 0:
                        print(3);
                        anim = RoomChangeAnimations[3];
                        break;
                }
                break;
            case Vector2Int v when v.Equals(new Vector2Int(1, 0)):
                switch (differrence)
                {
                    case -1:
                        print(4);
                        anim = RoomChangeAnimations[4];
                        break;

                    case 0:
                        print(5);
                        anim = RoomChangeAnimations[5];
                        break;
                    case 1:
                        print(6);
                        anim = RoomChangeAnimations[6];
                        break;
                }
                break;
            case Vector2Int v when v.Equals(new Vector2Int(-1, 0)):
                switch (differrence)
                {
                    case -1:
                        print(7);
                        anim = RoomChangeAnimations[7];
                        break;
                    case 0:
                        print(08);
                        anim = RoomChangeAnimations[8];
                        break;
                    case 1:
                        print(9);
                        anim = RoomChangeAnimations[9];
                        break;
                }
                break;
        }
        for(int i = 0; i < anim.animationPositions.Count; i++)
        {
            //TODO animation.setAnimaiton --> (anim.animationPositions,x)
            anime.SetBool("walk", false);
            anime.SetInteger("direction", anim.animDirection[i]);
            anime.SetBool("walk", true);
            while (transform.position != (Vector3)anim.animationPositions[i])
            {
                transform.position = Vector2.MoveTowards(transform.position, anim.animationPositions[i], speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        //voile baissé
        for (int i = 0;i<100;i++)
        {
            voile.color = new Color(0, 0, 0,i/100f);
            yield return new WaitForSeconds(0.001f);
        }
        Map.currentRoom.setActive(false);
        
        transform.position = newpos;
        point.transform.position = newpos;
        OnDoor = true;
        temp.setActive(true);
        Map.currentRoom = temp;
        //voile levé
        for (int i = 100; i >= 0; i--)
        {
            voile.color = new Color(0, 0, 0, i / 100f);
            yield return new WaitForSeconds(0.001f);
        }
        canmove = true;

    }
    void DrawPossibleMoves()
    {
        int k = moves;
        for (int i = -moves; i < moves + 1; i++)
        {
            for (int j = -Mathf.Abs(moves - Mathf.Abs(k)); j < Mathf.Abs(moves - Mathf.Abs(k) + 1); j++)
            {
                GUI.SetTile(new Vector3Int(i, j) + VectorHelper.Vector3Int(point.transform.position) - new Vector3Int(1, 1), normal);
                GUI.SetTile(new Vector3Int(i, j) + VectorHelper.Vector3Int(point.transform.position) - new Vector3Int(1, 1), normal);
            }
            k--;
        }
        moves--;
    }
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
                        Map.currentRoom.map[point.transform.position].block = false;
                        point.transform.position += deplacement;
                        Map.currentRoom.map[point.transform.position].block = true;
                        if (!checkDoors())
                        {
                            StartCoroutine(Move());
                        }
                        
                        if (!force)
                        {
                            Turn = 1;
                        }
                    }
                }
            }
        }
        else
        {
            Turn = 1;
            direction = dir;
            anime.SetInteger("direction", dir);

        }
    }
    //dammages is the direction were the player will be push after taking the damages
    //ammount is the amount of damages taken
    public bool checkDoors()
    {
        List<Vector3> newpos = new List<Vector3>() { new Vector3(0, -4), new Vector3(0, 4), new Vector3(-7, 0), new Vector3(7, 0) };
        List<Vector2Int> newdir = new List<Vector2Int>() { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };
        for (int i = 0; i < 4 && OnDoor == false; i++)
        {
            if (point.transform.position == Map.doors[i].transform.position && Map.doors[i].activeInHierarchy)
            {
                StartCoroutine(RoomChangeAnimation(newpos[i], newdir[i]));
                return true;
            }
        }
        return false;
    }
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
        

        List<Vector2Int> newdir = new List<Vector2Int>() { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };

        //CHANGE ROOM :
        //_____________________________________________________________________________________________//
        //Check if the player is on a door, it then set the bool ondoor true so it doesn't do it each frame, ondoor is false when the player mooves
        //for (int i = 0; i < 4 && OnDoor==false; i++)
        //{
        //    if (transform.position == Map.doors[i].transform.position && Map.doors[i].activeInHierarchy)
        //    {
        //        StartCoroutine(RoomChangeAnimation(newpos[i],newdir[i]));
        //    }
        //}
        //_____________________________________________________________________________________________//



        List<string> InputMouvementsNames = new List<string>() {"up", "down", "right","left" };
        int[] tem = { 2, 0, 1,3 };
        //A turn passes
        if (Turn > 0)
        {
            Turn -= 1;
        }
        //Move if input triggered
        for (int i = 0; i<4 && IsMooving == false && canmove ; i+=1)
        {
            if (_input.actions[InputMouvementsNames[i]].IsPressed())
            {
                Move((Vector2) newdir[i], i);
                break;
            }
        }
        //God Mod, instant change room with the arrows
        //for (int i = 0; i < 4 && IsMooving == false && canmove; i += 1)
        //{
        //    if (_input.actions["god"+i.ToString()].triggered)
        //    {
        //        GameObject.Find("Main Camera").transform.GetComponent<Map>().move(newdir[i]);
        //        break;
        //    }
        //}
        //Attack the ennemies in front of the player
        if (_input.actions["Attack"].triggered && !IsMooving)
        {
            arm.setAttack(tem[direction]);
            Turn = 1;
        }
       
        if (transform.position == point.transform.position)
        {
            IsMooving = false;
        }
        else
        {
            IsMooving = true;
        }
        if (canmove)
        {
            transform.position = Vector2.MoveTowards(transform.position, point.transform.position, speed * Time.deltaTime);
            anime.SetBool("walk", IsMooving);
        }


    }
}
[System.Serializable]
public class RoomChangeAnimationData
{
    public string name;
    public List<int> animDirection;
    public List<Vector2> animationPositions;
}