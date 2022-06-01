
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
    //the size of the map
    //the ennemies of the level
    //the amount of room we want of each type, x = type y = amount
    //in order of priority, the last one might not be in the donjon
    //apply only on node with no childs
    // 0 = chess room, 1 = ...
    public Level currentLevel;
    public static Level currentlevel;
    public GameObject currentRoomObject = null;
    //the cube that is copy paste to make the minimap
    public static GameObject cube;
    public GameObject Cube;
    //the chances of getting 0, 1, 2 or 3 rooms at each node
    [SerializeField]
    private List<int> MainChance = new List<int>();
    //the rate at wich the chances deacrease
    [SerializeField]
    private float rate;
    //the amount of iteration that produce the map
    [SerializeField]
    private int iterations;

    //force the amount of room wanted to be in the map
    [SerializeField]
    private bool IWantThoseRooms;
    //the text of different uses
    [SerializeField]
    private TextMeshPro text;

    //the minipa position 
    [SerializeField]
    private Vector2 minimapPosition;
    //the parrent of the minimap cubes
    [SerializeField]
    private GameObject minipmapDady;
    public GameObject background;

    //keep track of rooms with no childs
    private List<Room> EndRooms = new List<Room>();
    //not used anymore :(
    private int RedDisipation;
    //move the player to the room wanted new position being a direction vector


    //haut bas droite gauche
    [SerializeField]
    private Exit[] exits = new Exit[4];
    public static Exit[] exitstatic = new Exit[4];

    public void move(Vector2Int newPosition,Vector2 player = default(Vector2))
    {
        
        if (map[(currentRoom.position + newPosition).x][(currentRoom.position + newPosition).y] != null)
        {
            StartCoroutine(moove(newPosition, player));
        }
    }
    //animated 
    IEnumerator moove(Vector2Int finalpos,Vector2 player)
    {
        var temp = map[currentRoom.position.x + (int)finalpos.x][currentRoom.position.y + (int)finalpos.y];
        //temp.setActive(true);
        
        Dictionary<Vector2Int, Vector3> pos = new Dictionary<Vector2Int, Vector3>();
        pos.Add(new Vector2Int(0, 1), new Vector3(0, 8,-10));
        pos.Add(new Vector2Int(0, -1), new Vector3(0, -8, -10));
        pos.Add(new Vector2Int(1, 0), new Vector3(14, 0, -10));
        pos.Add(new Vector2Int(-1, 0), new Vector3(-14, 0, -10));
        temp.background.transform.position = (Vector2)pos[finalpos];
        temp.setActive(true);
        while (gameObject.transform.position != pos[finalpos])
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pos[finalpos], 15 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        PlayerMovement.player.transform.position = player;
        PlayerMovement.points.transform.position = player;
        currentRoom.setActive(false);
        gameObject.transform.position = new Vector3(0, 0, -10);
        temp.background.transform.position = new Vector3(0, 0);
        currentRoom = temp;
    }
    void Awake()
    {
        cube = Cube;
        exitstatic = exits;
        currentlevel = currentLevel;
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
        for (int i = 0; i < currentLevel.size.x; i++)
        {
            map.Add(new List<Room>());
            for (int k = 0; k < currentLevel.size.y; k++)
            {
                map[i].Add(null);
            }
        }
        //generate the five mains rooms
        map[currentLevel.size.x / 2][currentLevel.size.y / 2] = new Room(new Vector2Int(currentLevel.size.x / 2, currentLevel.size.y / 2), currentLevel.ennemies,background,null, currentLevel.maxheight / 2);
        List<Vector2Int> Neightboors = new List<Vector2Int>() { new Vector2Int(currentLevel.size.x / 2, currentLevel.size.x / 2 + 1), new Vector2Int(currentLevel.size.x / 2 + 1, currentLevel.size.x / 2), new Vector2Int(currentLevel.size.x / 2, currentLevel.size.x / 2 - 1), new Vector2Int(currentLevel.size.x / 2 - 1, currentLevel.size.x / 2) };
        for (int i = 0; i < 4; i++)
        {
            string al = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            map[Neightboors[i].x][Neightboors[i].y] = new Room(Neightboors[i], currentLevel.ennemies, background, map[currentLevel.size.x / 2][currentLevel.size.y / 2],currentLevel.maxheight/2);
            map[Neightboors[i].x][Neightboors[i].y].name = al[Random.Range(0, al.Length)].ToString();
            map[currentLevel.size.x / 2][currentLevel.size.y / 2].enfants.Add(map[Neightboors[i].x][Neightboors[i].y]);
        }
        //set the position of the minimap parrent to fit with the center of the minimap
        minipmapDady.transform.position = (Vector2)new Vector2Int(currentLevel.size.x / 2, currentLevel.size.y / 2);
        currentRoom = map[currentLevel.size.x / 2][currentLevel.size.y / 2];
        //generate the map with the given parameter
        Generate();
        //Draw it for the mini map
        Draw(map[currentLevel.size.x / 2][currentLevel.size.y / 2]);
        //set the current room as active
        currentRoom.setActive(true);
        //reposition the minimap
        minipmapDady.transform.position = minimapPosition;
        minipmapDady.transform.localScale = (Vector2)new Vector2(0.3f, 0.3f);
        SetEndRooms(currentLevel.RoomTypes);
    }
    //Set the end room with the RoomTypes parametres
    private void SetEndRooms(List<Vector2Int> RoomTypes)
    {
        RoomTypes = currentLevel.RoomTypes;
        while (true)
        {
                try
                {
                    if (RoomTypes[0].y < 1)
                    {
                        RoomTypes.RemoveAt(0);
                    }
                }
                catch { }
            
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
        temp.GetComponent<SpriteRenderer>().color = currentLevel.etage[First.height];
        Vector2[] pos = { new Vector2(0.5f, 0), new Vector2(-0.5f, 0), new Vector2(0, 0.5f), new Vector2(0, -0.5f) };
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
        //set the current population and the childs
        List<Room> current = new List<Room>() { map[currentLevel.size.x / 2][currentLevel.size.x / 2 + 1], map[currentLevel.size.x / 2][currentLevel.size.x / 2 - 1], map[currentLevel.size.x / 2 - 1][currentLevel.size.x / 2], map[currentLevel.size.x / 2 + 1][currentLevel.size.x / 2] };
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
        for (int i = 0; i < iterations && currentRooms < currentLevel.Rooms; i++)
        {
            //tem keep track of the count of the current population since it will change
            int tem = current.Count;

            for (int k = 0; k < tem && currentRooms < currentLevel.Rooms; k++)
            {
                //"availables" return the availables neighboors
                List<Vector2Int> paths = availables(current[0].position);
                //"chances" is currently the chance of having a "Roomsamount" of child
                int Roomsamount = Percentage(MainChance);

                int[] heights = {0,0,0 };
                if (current[0].height > 0)
                {
                    heights[0] = -1;
                }
                if (current[0].height < currentLevel.maxheight-1)
                {
                    heights[1] = 1;
                }
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
                    var height = heights[Random.Range(0, heights.Length)];
                    if (height == -1 && ran == current[0].position + new Vector2Int(0, 1)){height = 0;}
                    else if (height == 1 && ran == current[0].position + new Vector2Int(0, -1)){height = 0;}
                    
                    Room temp = new Room(ran, currentLevel.ennemies, background,current[0],current[0].height + height);
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

            if (currentRooms >= currentLevel.Rooms)
            {
                break;
            }
        }
        //if IWantThoseRooms and the amount of rooms wanted is not check, we redo the process until it works
        print(EndRooms.Count);
        if (IWantThoseRooms && currentLevel.Rooms > currentRooms)
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

    public static void Listprint(List<int> all)
    {

        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
    public static void Listprint(List<Vector2Int> all)
    {

        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
    public static void Listprint(List<Vector3Int> all)
    {

        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
    public static void Listprint(List<Vector2> all)
    {
        string final = "";
        for (int i = 0; i < all.Count; i += 1)
        {
            final += " : " + all[i].ToString();
        }
        print(final);
    }
    public static void Listprint(List<string> all)
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

    //public List<Vector2> mapp = new List<Vector2>();

    public List<GameObject> ennemies = new List<GameObject>();
    public int mode = 0;
    //public Dictionary<Vector2, Attack> dammages = new Dictionary<Vector2, Attack>();

    public Dictionary<Vector2,Tile> map = new Dictionary<Vector2, Tile>();

    public GameObject background;
    public int height;
    public Room(Vector2Int position, List<Ennemie> ennemies, GameObject background, Room father = null, int height = 0)
    {

        this.position = position;
        this.father = father;
        this.height = height;
       
        var vect1 = new Vector2(-8, 5);
        for (int i = 0; i < 17; i++)
        {
            for (int k = 0; k < 11; k++)
            {
                map.Add(vect1, new Tile());
                vect1.x += 1;
                if (vect1.x == 9)
                {
                    vect1.x = -8;
                    vect1.y -= 1;
                }
            }
        }
        this.background = GameObject.Instantiate(background);
        background.SetActive(false);
        for (int i = 0; i < Random.Range(Map.currentlevel.ennemiesAmmount.x, Map.currentlevel.ennemiesAmmount.y); i++)
        {
            var vect = new Vector3(Random.Range(-7, 7), Random.Range(4, -4));
            var temo = GameObject.Instantiate(ennemies[Random.Range(0, ennemies.Count)].me, vect, Quaternion.identity);
            temo.transform.parent = this.background.transform;
            //temo.SetActive(false);
            this.ennemies.Add(temo);
            map[vect].block = true;

        }
    }
    //return a colision map for ennemies
    public List<List<Vector3>> AstarMap()
    {
        List<List<Vector3>> pos = new List<List<Vector3>>();
        var vect1 = new Vector2(-8, 5);
        for(int i  = 0; i < 16;i++)
        {
            pos.Add(new List<Vector3>());
            for (int k = 0; k < 10; k++)
            {
                if (map[vect1].block || map[vect1].ennemiesamo)
                {
                    pos[i].Add(new Vector3(vect1.x, vect1.y,1));
                }
                else
                {
                    pos[i].Add(new Vector3(vect1.x, vect1.y, 0));
                }
                vect1.x += 1;
                if (vect1.x == 9)
                {
                    vect1.x = -8;
                    vect1.y -= 1;
                }
            }
        }
        return pos;
    }
     public List<Vector2Int> AstarMapVector2Int()
    {
        List<Vector2Int> pos = new List<Vector2Int>();
        var vect1 = new Vector2Int(-8, 5);
        for(int i  = 0; i < 16;i++)
        {
            for (int k = 0; k < 10; k++)
            {
                if (!(map[vect1].block || map[vect1].ennemiesamo))
                {
                    
                    pos.Add(new Vector2Int(vect1.x, vect1.y));
                }
                vect1.x += 1;
                if (vect1.x == 9)
                {
                    vect1.x = -8;
                    vect1.y -= 1;
                }
            }
        }
        return pos;
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
            temp.transform.parent = background.transform;
            ennemies.Add(temp);
            map[new Vector2(0, 0)].block = true;
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
                for (int j = 0; j < Map.exitstatic[i].sprites.Count; j++)
                {
                    Map.exitstatic[i].sprites[j].SetActive(false);
                }
            }
            List<Vector2Int> neigth = new List<Vector2Int>() { position + new Vector2Int(0, 1), position + new Vector2Int(0, -1), position + new Vector2Int(1, 0), position + new Vector2Int(-1, 0) };
            List<int> done = new List<int>(){ 0, 1, 2, 3 };

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Debug.Log(height - enfants[i].height);
                }
                catch { }
                
                for (int k = 0; k < enfants.Count; k++)
                {
                   
                    if (enfants[k].position == neigth[i])
                    {
                        done.Remove(i);
                        Map.doors[i].SetActive(true);
                        switch (i, height - enfants[k].height)
                        {
                            //couloir haut bas:
                            case (<2,0):
                                Map.exitstatic[i].sprites[2].SetActive(true);
                                break;
                            //monte haut
                            case (0, <0):
                                Map.exitstatic[i].sprites[1].SetActive(true);
                                break;
                            //descend bas
                            case (1, >0):
                                Map.exitstatic[i].sprites[1].SetActive(true);
                                break;
                            //couloir gauche droite
                            case (2, 0):
                                Map.exitstatic[i].sprites[2].SetActive(true);
                                break;
                            case ( >1,0):
                                Map.exitstatic[i].sprites[2].SetActive(true);
                                break;
                            //haut gauche droite
                            case ( > 1, <0):
                                Map.exitstatic[i].sprites[3].SetActive(true);
                                break;
                            //bas gauche droite
                            case ( > 1, > 0):
                                Map.exitstatic[i].sprites[1].SetActive(true);
                                break;
                        }
                    }
                }
                try
                {
                    if (father.position == neigth[i])
                    {
                        done.Remove(i);
                        Map.doors[i].SetActive(true);
                        switch (i, height - father.height)
                        {
                            //couloir haut bas:
                            case ( < 2, 0):
                                Map.exitstatic[i].sprites[2].SetActive(true);
                                break;
                            //monte haut
                            case (0, < 0):
                                Map.exitstatic[i].sprites[1].SetActive(true);
                                break;
                            //descend bas
                            case (1, > 0):
                                Map.exitstatic[i].sprites[1].SetActive(true);
                                break;
                            //couloir gauche droite
                            case ( > 1, 0):
                                Map.exitstatic[i].sprites[2].SetActive(true);
                                break;
                            //haut gauche droite
                            case ( > 1, < 0):
                                Map.exitstatic[i].sprites[3].SetActive(true);
                                break;
                            //bas gauche droite
                            default:
                                Map.exitstatic[i].sprites[1].SetActive(true);
                                break;
                        }
                    }
                }
                catch
                {
                    //print(map[currentRoom.x][currentRoom.y].father);
                }
            }
            for (int i = 0; i < done.Count; i++)
            {
                Map.exitstatic[done[i]].sprites[0].SetActive(true);
            }
        }
        else
        {
            MapPiece.GetComponent<SpriteRenderer>().color = Map.currentlevel.etage[height];
        }
        background.SetActive(enable);
        //for (int i = 0; i < ennemies.Count; i++)
        //{
        //    ennemies[i].SetActive(enable);
        //}

    }
}
[System.Serializable]
public class Exit
{
    public string name;
    //mur-bas-couloir-haut
    //mur-escalier(haut ou bas)-couloir-rien
    public List<GameObject> sprites;

}

public class Tile 
{
    public Vector2 position;
    public List<Attack> attacks = new List<Attack>();
    public bool block;
    public bool ennemiesamo;
}