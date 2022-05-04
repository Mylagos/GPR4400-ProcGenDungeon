using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    List<List<Room>> map = new List<List<Room>>();
    [SerializeField]
    private int rooms;
    [SerializeField]
    private GameObject Cube;
    [SerializeField]
    private List<int> MainChance = new List<int>();
    [SerializeField]
    private float rate;
    [SerializeField]
    private int iterations;
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private int RedDisipation;
    //force the amount of room wanted to be in the map
    [SerializeField]
    private bool IWantThoseRooms;
    void Awake()
    {
        for (int i = 0; i < size.x; i++)
        {
            map.Add(new List<Room>());
            for (int k = 0; k < size.y; k++)
            {
                map[i].Add(null);
            }
        }
        //generate the five mains rooms
        map[size.x/2][size.y / 2] = new Room(new Vector2Int(size.x / 2, size.y / 2));
        List<Vector2Int> Neightboors = new List<Vector2Int>() { new Vector2Int(size.x / 2, size.x / 2 + 1), new Vector2Int(size.x / 2 + 1, size.x / 2), new Vector2Int(size.x / 2, size.x / 2-1), new Vector2Int(size.x / 2 - 1, size.x / 2) };
        for (int i = 0; i< 4; i++)
        {
            map[Neightboors[i].x][Neightboors[i].y] = new Room(Neightboors[i], map[5][5]);
            map[size.x / 2][size.y / 2].enfants.Add(map[Neightboors[i].x][Neightboors[i].y]);
        }
        //generate the rest with the given parameter
        gameObject.transform.position = (Vector3) new Vector3Int(size.x / 2, size.y / 2,-10);
        Generate();
        //Draw it
        Draw(1, new Color(0,0,0), map[size.x / 2][size.y / 2]);

    }
    //Dessine la map pour donner un aperçue (recurcif)
    void Draw(int mode, Color color, Room First = null,int direction = 0 )
    {
        if (mode == 0)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int k = 0; k < 11; k++)
                {
                    if (map[i][k] != null)
                    {
                        GameObject temp = GameObject.Instantiate(Cube, (Vector2)map[i][k].position, Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            GameObject temp = GameObject.Instantiate(Cube,(Vector2) First.position, Quaternion.identity);

            temp.GetComponent<SpriteRenderer>().color = color;
            Debug.Log(First.enfants.Count);
            if (direction == 0)
            {
                temp = GameObject.Instantiate(Cube, (Vector2)First.position +new Vector2(0.5f,0), Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                temp.transform.localScale -= new Vector3(0, 0.25f, 0);
            }
            if (direction == 1)
            {
                temp = GameObject.Instantiate(Cube, (Vector2)First.position - new Vector2(0.5f, 0), Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                temp.transform.localScale -= new Vector3(0, 0.25f, 0);
            }
            if (direction == 2)
            {
                temp = GameObject.Instantiate(Cube, (Vector2)First.position + new Vector2(0, 0.5f), Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
                temp.transform.localScale -= new Vector3(0.25f, 0, 0);
            }
            if (direction == 3)
            {
                temp = GameObject.Instantiate(Cube, (Vector2)First.position - new Vector2(0, 0.5f), Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                temp.transform.localScale -= new Vector3(0.25f, 0, 0);
            }
            for (int i = 0; i < First.enfants.Count;i++)
            {
                int model = 0;
                if (First.enfants[i].position.x +1== First.position.x)
                {
                    model = 0;
                }
                if (First.enfants[i].position.x -1== First.position.x)
                {
                    model = 1;
                }
                if (First.enfants[i].position.y+1 == First.position.y)
                {
                    model = 2;
                }
                if (First.enfants[i].position.y-1 == First.position.y)
                {
                    model = 3;
                }
                Draw(1, color + new Color((float)1 / RedDisipation, 0, 0), First.enfants[i],model);
            }

        }
       
    }

    //generate the map
    Room RandomChild()
    {
        Room temp = map[size.x / 2][size.y / 2];
        for (int i = 0;i< Random.Range(0, iterations / 2); i++)
        {
            try
            {
                temp = temp.enfants[Random.Range(0, temp.enfants.Count)];
            }
            catch { break; }
            
        }
        return temp;
    }
    //generate the maps using the parameters given
    void Generate(bool redo = false, int currentRooms = 0)
    {
        //set the current population and the childs
        List<Room> current = new List<Room>() { map[size.x / 2][size.x / 2 + 1], map[size.x / 2][size.x / 2 - 1], map[size.x / 2 - 1][size.x / 2], map[size.x / 2 + 1][size.x / 2] };
        List<Room> child = new List<Room>();
        //we add a random child her to avoid max depth recursion when using the IWantThoseRooms function
        if (redo)
        {
            current.Add(RandomChild());
        }

        //loop for a certain depth and terminate if there is anought rooms
        for (int i = 0; i < iterations && currentRooms < rooms; i++)
        {
            //tem keep track of the count of the current population since it will change
            int tem = current.Count;
            
            for (int k = 0; k < tem && currentRooms < rooms; k++) {
                //"availables" return the availables neighboors
                List<Vector2Int> paths = availables(current[0].position);
                //"chances" is currently the chance of having a "Roomsamount" of child
                int Roomsamount = Percentage(MainChance);
                //if the current room is the last of the population and no one had child, it will have at least one
                if (current.Count == 1 && child.Count == 0)
                {
                    List<int> chances = new List<int>() { 1,1,1,1,1,1, 2,2, 3 };
                    Roomsamount = chances[Random.Range(0, chances.Count)];
                }
                //else if there is less path than the choosen roomsamout, the rooms amount will be the paths amount
                if (Roomsamount > paths.Count) {
                    Roomsamount = paths.Count;
                }
                //for the roomsamount
                for (int j = 0; j < Roomsamount; j++)
                {
                    //we take a random path
                    Vector2Int ran = paths[Random.Range(0, paths.Count)];
                    //we create a new room at this position
                    Room temp = new Room(ran, current[0]);
                    //we add the new room to the map and as the current room's child
                    map[ran.x][ran.y] = temp;
                    current[0].enfants.Add(temp);
                    //we increase the amount of rooms and add the child to child
                    currentRooms += 1;
                    child.Add(temp);
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
          
            if (currentRooms == rooms)
            {
                break;
            }
        }
        //if IWantThoseRooms and the amount of rooms wanted is not check, we redo the process until it works
        if (IWantThoseRooms && rooms > currentRooms)
        {
            Generate(true, currentRooms);
        }
    }
    //retourne un nombre par weighted random par la list chances
    int Percentage(List<int> chances)
    {
        List<int> ratio = new List<int>();
        for (int i = 0; i< chances.Count; i++)
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
        for (int i = 0; i< father.Count; i++)
        {
            clo.Add(father[i]);
        }
        return clo;

    }
    //check la disponibilité
    List<Vector2Int> availables(Vector2Int pos)
    {
        
        List<Vector2Int> paths = new List<Vector2Int>();
        Vector2Int[] vectors = { pos + new Vector2Int(0, 1), pos + new Vector2Int(1, 0), pos + new Vector2Int(0, -1), pos + new Vector2Int(-1, 0) };
        for (int i = 0; i < 4; i++)
        {
            try
            {
                if (map[vectors[i].x][vectors[i].y] == null)
                {
                    paths.Add(vectors[i]);
                }
            }catch{}
           
        }
        return paths;
    }
}
public class Room
{
    public Vector2Int position;
    public Room father;
    public List<Room> enfants = new List<Room>();
    public bool Dead = false;
    public Room(Vector2Int position, Room father = null)
    {
        this.position = position;
        this.father = father;
    }
}