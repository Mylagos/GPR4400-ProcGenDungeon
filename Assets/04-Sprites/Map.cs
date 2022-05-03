using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    // Start is called before the first frame update
    List<List<Room>> map = new List<List<Room>>();
    int rooms;
    GameObject Cube;

    public Map()
    {
    }
        public Map(int rooms, GameObject Cub)
    {
        this.rooms = rooms;
        for (int i = 0; i < 11; i++)
        {
            map.Add(new List<Room>());
            for (int k = 0; k < 11; k++)
            {
                map[i].Add(null);
            }
        }
        Cube = Cub;

        map[5][5] = new Room(new Vector2Int(5, 5));
        

        map[5][4] = new Room(new Vector2Int(5, 4), map[5][5]);
        map[5][6] = new Room(new Vector2Int(5, 6), map[5][5]);
        map[6][5] = new Room(new Vector2Int(6, 5), map[5][5]);
        map[4][5] = new Room(new Vector2Int(4, 5), map[5][5]);

        map[5][5].enfants.Add(map[5][4]);
        map[5][5].enfants.Add(map[4][5]);
        map[5][5].enfants.Add(map[5][6]);
        map[5][5].enfants.Add(map[6][5]);


        Generate();
        Draw(1, new Color(0,0,0),map[5][5]);
    }
//Dessine la map pour donner un aperçue (recurcif)
    void Draw(int mode, Color color, Room First = null)
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
            for (int i = 0; i < First.enfants.Count;i++)
            {
                Draw(1, color+new Color((float)1/10, (float)1 / 10, (float)1 / 10), First.enfants[i]);
            }

        }
       
    }

    //generate the map
    void Generate()
    {

        //current is the current generation and child will be its childs
        List<Room> current = new List<Room>() { map[6][5], map[5][6], map[5][4], map[4][5] };
        List<Room> child = new List<Room>();
        //keep track of the amount of room  added
        int currentRooms = 0;
        //loop for a certain depth and terminate if there is anought rooms
        for (int i = 0; i < 8 && currentRooms != rooms; i++)
        {
            //tem keep track of the count of the current population since it will change
            int tem = current.Count;
            
            for (int k = 0; k < tem && currentRooms != rooms; k++) {
                //"availables" return the availables neighboors
                List<Vector2Int> paths = availables(current[0].position);
                //"chances" is currently the chance of having a "Roomsamount" of child
                List<int> chances = new List<int>() { 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 3 };
                int Roomsamount = chances[Random.Range(0, chances.Count)];
                //if the current room is the last of the population and no one had child, it will have at least one
                if (current.Count == 1 && child.Count == 0)
                {
                    chances = new List<int>() { 1,1,1,1,1,1, 2,2, 3 };
                    Roomsamount = chances[Random.Range(0, chances.Count)];
                }
                //else if there is less path than the choosen roomsamout, the rooms amount will be the paths amount
                else if (Roomsamount > paths.Count) {
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
                    rooms += 1;
                    child.Add(temp);
                }
                //remove the first current since we always only deal with the first one
                current.RemoveAt(0);
            }
            //clone the child to be the next current and clear the child
            current = clone(child);
            child.Clear();
        }
    }
    List<Room> clone(List<Room> father)
    {
        List<Room> clo = new List<Room>();
        for (int i = 0; i< father.Count; i++)
        {
            clo.Add(father[i]);
        }
        return clo;

    }
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