using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
    //the map
    static List<List<Room>> map = new List<List<Room>>();
    //the doors as game object
    public static List<GameObject> doors = new List<GameObject>();    
    //keep track of the current room so every script can access it
    public static Room currentRoom;
    //amount of rooms wanted
    [SerializeField]
    private int rooms;
    //the cube that is copy paste to make the minimap
    [SerializeField]
    private GameObject Cube;
    //the chances of getting 0, 1, 2 or 3 rooms at each node
    [SerializeField]
    private List<int> MainChance = new List<int>();
    //the rate at wich the chances deacrease
    [SerializeField]
    private float rate;
    //the amount of iteration that produce the map
    [SerializeField]
    private int iterations;
    //the size of the map
    [SerializeField]
    private Vector2Int size;
    //force the amount of room wanted to be in the map
    [SerializeField]
    private bool IWantThoseRooms;
    //the text of different uses
    [SerializeField]
    private TextMeshPro text;
    //the ennemies of the level
    [SerializeField]
    private List<GameObject> Ennemies = new List<GameObject>();
    //the minipa position 
    [SerializeField]
    private Vector2 minimapPosition;
    //the parrent of the minimap cubes
    [SerializeField]
    private GameObject minipmapDady;
    //the amount of room we want of each type, x = type y = amount
    //in order of priority, the last one might not be in the donjon
    //apply only on node with no childs
    // 0 = chess room, 1 = ...
    [SerializeField]
    private List<Vector2Int> RoomsTypes;
    //keep track of rooms with no childs
    private List<Room> EndRooms = new List<Room>();
    //not used anymore :(
    private int RedDisipation;
    //move the player to the room wanted new position being a direction vector
    public static void move(Vector2Int newPosition)
    {
        if (map[(currentRoom.position + newPosition).x][(currentRoom.position + newPosition).y] != null)
        {
            currentRoom.setActive(false);
            currentRoom.position = currentRoom.position + newPosition;
            currentRoom = map[currentRoom.position.x][currentRoom.position.y];
            currentRoom.setActive(true);
        }
    }

    void Awake()
    {
        
        //reset static variables
        map = new List<List<Room>>();
        doors = new List<GameObject>();
        //get the door as static object so every script can take then from here
        List<string> doorsnames = new List<string>() { "Door", "Door1", "Door2", "Door3" };
        for (int i = 0; i < 4; i += 1)
        {
            doors.Add(GameObject.Find(doorsnames[i]));
        }
        //generate the map 2d array
        for (int i = 0; i < size.x; i++)
        {
            map.Add(new List<Room>());
            for (int k = 0; k < size.y; k++)
            {
                map[i].Add(null);
            }
        }
        //generate the five mains rooms
        map[size.x / 2][size.y / 2] = new Room(new Vector2Int(size.x / 2, size.y / 2), Ennemies);
        List<Vector2Int> Neightboors = new List<Vector2Int>() { new Vector2Int(size.x / 2, size.x / 2 + 1), new Vector2Int(size.x / 2 + 1, size.x / 2), new Vector2Int(size.x / 2, size.x / 2 - 1), new Vector2Int(size.x / 2 - 1, size.x / 2) };
        for (int i = 0; i < 4; i++)
        {
            string al = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            map[Neightboors[i].x][Neightboors[i].y] = new Room(Neightboors[i], Ennemies, map[size.x / 2][size.y / 2]);
            map[Neightboors[i].x][Neightboors[i].y].name = al[Random.Range(0, al.Length)].ToString();
            map[size.x / 2][size.y / 2].enfants.Add(map[Neightboors[i].x][Neightboors[i].y]);
        }
        //set the position of the minimap parrent to fit with the center of the minimap
        minipmapDady.transform.position = (Vector2)new Vector2Int(size.x / 2, size.y / 2);
        currentRoom = map[size.x / 2][size.y / 2];
        //generate the map with the given parameter
        Generate();
        //Draw it for the mini map
        Draw(map[size.x / 2][size.y / 2]);
        //set the current room as active
        currentRoom.setActive(true);
        //reposition the minimap
        minipmapDady.transform.position = minimapPosition;
        minipmapDady.transform.localScale = (Vector2)new Vector2(0.3f, 0.3f);

        SetEndRooms(RoomsTypes);
    }
    //Set the end room with the RoomTypes parametres
    private void SetEndRooms(List<Vector2Int> RoomTypes)
    {
        while (true)
        {
            if (RoomTypes[0].y < 1)
            {
                RoomTypes.RemoveAt(0);
            }
            if (RoomTypes.Count < 1 || EndRooms.Count < 1)
            {
                break;
            }
            var ran = Random.Range(0, EndRooms.Count);
            EndRooms[ran].setMode(RoomTypes[0].x);
            RoomTypes[0] -= new Vector2Int(0, 1);
            EndRooms.RemoveAt(ran);
        }
    }
    //Dwaw the minimap
    void Draw(Room First, int direction = 0)
    {
        //generate the cube and the branches
        GameObject temp = GameObject.Instantiate(Cube, (Vector2)First.position, Quaternion.identity, minipmapDady.transform);
        First.MapPiece = temp;
        temp.GetComponent<SpriteRenderer>().color = new Color(1,0,0);
        Vector2[] pos = { new Vector2(0.5f, 0), new Vector2(-0.5f, 0), new Vector2(0, 0.5f),  new Vector2(0, -0.5f) };
        Vector3[] newsize = { new Vector3(0, 0.25f, 0), new Vector3(0, 0.25f, 0), new Vector3(0.25f, 0, 0), new Vector3(0.25f, 0, 0) };
        for (int i = 0; i < 4; i++)
        {
            if (direction == i)
            {
                temp = GameObject.Instantiate(Cube, (Vector2)First.position + pos[i], Quaternion.identity, minipmapDady.transform);
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                temp.transform.localScale -= newsize[i];
            }
        }
        //generate the rooms next to the current room recurcivly
        for (int i = 0; i < First.enfants.Count; i++)
        {
            int model = 0;
            if (First.enfants[i].position.x + 1 == First.position.x)
            {
                model = 0;
            }
            if (First.enfants[i].position.x - 1 == First.position.x)
            {
                model = 1;
            }
            if (First.enfants[i].position.y + 1 == First.position.y)
            {
                model = 2;
            }
            if (First.enfants[i].position.y - 1 == First.position.y)
            {
                model = 3;
            }
            Draw(First.enfants[i], model);
        }

    }
   
    //generate the maps using the parameters given
    void Generate(bool redo = false, int currentRooms = 0)
    {
        string al = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //set the current population and the childs
        List<Room> current = new List<Room>() { map[size.x / 2][size.x / 2 + 1], map[size.x / 2][size.x / 2 - 1], map[size.x / 2 - 1][size.x / 2], map[size.x / 2 + 1][size.x / 2] };
        List<Room> child = new List<Room>();
        //we add a random child from the EndRooms here to avoid max depth recursion when using the IWantThoseRooms function
        if (redo)
        {
            current.Clear();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var temp = Random.Range(0, EndRooms.Count);
                    current.Add(EndRooms[temp]);
                    EndRooms.RemoveAt(0);
                }
                catch { }
            }
        }

        //loop for a certain depth and terminate if there is anought rooms
        for (int i = 0; i < iterations && currentRooms < rooms; i++)
        {
            //tem keep track of the count of the current population since it will change
            int tem = current.Count;

            for (int k = 0; k < tem && currentRooms < rooms; k++)
            {
                //"availables" return the availables neighboors
                List<Vector2Int> paths = availables(current[0].position);
                //"chances" is currently the chance of having a "Roomsamount" of child
                int Roomsamount = Percentage(MainChance);
                //if the current room is the last of the population and no one had child, it will have at least one
                if (current.Count == 1 && child.Count == 0)
                {
                    List<int> chances = new List<int>() { 1, 1, 1, 1, 1, 1, 2, 2, 3 };
                    Roomsamount = chances[Random.Range(0, chances.Count)];
                }
                //else if there is less path than the choosen roomsamout, the rooms amount will be the paths amount
                if (Roomsamount > paths.Count)
                {
                    Roomsamount = paths.Count;
                }
                //for the roomsamount
                for (int j = 0; j < Roomsamount; j++)
                {
                    //we take a random path
                    Vector2Int ran = paths[Random.Range(0, paths.Count)];
                    //we create a new room at this position
                    Room temp = new Room(ran, Ennemies, current[0]);
                    temp.name = al[Random.Range(0, al.Length)].ToString();
                    //we add the new room to the map and as the current room's child
                    current[0].enfants.Add(temp);
                    map[ran.x][ran.y] = temp;
                    //we increase the amount of rooms and add the child to child
                    currentRooms += 1;
                    child.Add(temp);
                    paths.Remove(ran);
                }
                if (Roomsamount == 0)
                {
                    EndRooms.Add(current[0]);
                }

                //remove the first current since we always only deal with the first one
                current.RemoveAt(0);

            }
            for (int k = 0; k < MainChance.Count; k++)
            {
                MainChance[k] -= (int)(k * rate);
            }
            //clone the child to be the next current and clear the child
            current = clone(child);
            child.Clear();

            if (currentRooms >= rooms)
            {
                break;
            }
        }
        //if IWantThoseRooms and the amount of rooms wanted is not check, we redo the process until it works
        print(EndRooms.Count);
        if (IWantThoseRooms && rooms > currentRooms)
        {
            Generate(true, currentRooms);
        }
        print(currentRooms); ;


    }
    //retourne un nombre par weighted random par la list chances
    int Percentage(List<int> chances)
    {
        List<int> ratio = new List<int>();
        for (int i = 0; i < chances.Count; i++)
        {
            for (int k = 0; k < chances[i]; k++)
            {
                ratio.Add(i);
            }
        }
        return ratio[Random.Range(0, ratio.Count)];
    }
    //clones une list de Room
    List<Room> clone(List<Room> father)
    {
        List<Room> clo = new List<Room>();
        for (int i = 0; i < father.Count; i++)
        {
            clo.Add(father[i]);
        }
        return clo;

    }
    //check la disponibilité des salles allentours
    List<Vector2Int> availables(Vector2Int pos)
    {

        List<Vector2Int> paths = new List<Vector2Int>();
        Vector2Int[] vectors = { pos + new Vector2Int(-1, 0), pos + new Vector2Int(1, 0), pos + new Vector2Int(0, -1), pos + new Vector2Int(0, 1) };
        for (int i = 0; i < 4; i++)
        {
            try
            {
                if (map[vectors[i].x][vectors[i].y] == null)
                {
                    paths.Add(vectors[i]);
                }
            }
            catch { }

        }
        return paths;
    }

    void Listprint(List<int> all)
    {

        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
    void Listprint(List<Vector2> all)
    {
        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
    void Listprint(List<string> all)
    {
        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
}

public class Room
{
    public Vector2Int size;
    public GameObject MapPiece;
    public string name = "0";
    public Vector2Int position;
    public Room father;
    public List<Room> enfants = new List<Room>();
    public GameObject Tilemap;
    public List<Vector2> mapp = new List<Vector2>();
    public List<GameObject> ennemies = new List<GameObject>();
    public int mode = 0;
    public Room(Vector2Int position, List<GameObject> ennemies, Room father = null, Vector2Int size = default(Vector2Int), GameObject Tilemap = null)
    {

        this.position = position;
        this.father = father;
        this.size = size;
        this.Tilemap = Tilemap;

        for (int i = 0; i < Random.Range(0, 4); i++)
        {
            var temo = GameObject.Instantiate(ennemies[Random.Range(0, ennemies.Count)], new Vector3(Random.Range(-7, 7), Random.Range(4, -4)), Quaternion.identity);
            temo.SetActive(false);
            this.ennemies.Add(temo);
        }

    }
    //set the mode of the room : 0 = chessroom, 1 = ... 
    public void setMode(int i)
    {
        mode = i;
        for (int j = 0; j < ennemies.Count; j++)
        {
            GameObject.Destroy(ennemies[j]);
        }
        if (mode == 0)
        {

            ennemies.Clear();
            var temp = GameObject.Instantiate(GameObject.Find("chest"), new Vector3(0, 0, 0), Quaternion.identity);
            temp.SetActive(false);
            ennemies.Add(temp);
            mapp.Add(new Vector3(0, 0, 0));
        }

    }
    //set the room room active or inactive
    public void setActive(bool enable)
    {

        if (enable == true)
        {
            MapPiece.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
            for (int i = 0; i < 4; i += 1)
            {
                Map.doors[i].SetActive(false);
            }
            List<Vector2Int> neigth = new List<Vector2Int>() { position + new Vector2Int(0, 1), position + new Vector2Int(0, -1), position + new Vector2Int(1, 0), position + new Vector2Int(-1, 0) };
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < enfants.Count; k++)
                {
                    if (enfants[k].position == neigth[i])
                    {
                        Map.doors[i].SetActive(true);
                    }
                }
                try
                {
                    if (father.position == neigth[i])
                    {
                        Map.doors[i].SetActive(true);
                    }
                }
                catch
                { //print(map[currentRoom.x][currentRoom.y].father);
                }
            }
        }
        else
        {
            MapPiece.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }


        for (int i = 0; i < ennemies.Count; i++)
        {
            ennemies[i].SetActive(enable);
        }

    }
}