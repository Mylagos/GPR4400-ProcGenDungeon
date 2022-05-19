using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public Weapon weapon;
    public int whom;
    public int frameLeft;
    public int frames;
    public Vector2 initialPosition;
    public List<List<Vector2Int>> attack;
    public List<Vector2> temp = new List<Vector2>();
    public List<GameObject> tempAnim = new List<GameObject>();
    public Attack(List<List<Vector2Int>> attack, Vector2 initialPosition, Weapon weapon, int whom)
    {
        this.attack = attack;
        frameLeft = attack.Count - 1;
        frames = attack.Count - 1;
        this.initialPosition = initialPosition;
        this.weapon = weapon;
        this.whom = whom;
    }
    public Attack(Attack attack, Vector2 recule)
    {
        initialPosition = recule;
        weapon = attack.weapon;
        whom = attack.whom;
    }
    public void destroyAt()
    {
        for (int i = 0; i < tempAnim.Count ; i++)
        {
            Map.currentRoom.map[temp[i]].ennemiesamo = false;
            Map.currentRoom.map[temp[i]].attacks.RemoveAt(0);
            //Map.currentRoom.dammages.Remove(temp[i]);
            GameObject.Destroy(tempAnim[i]);
        }
        tempAnim.Clear();
        temp.Clear();
    }
    public bool update()
    {

        destroyAt();
        for (int i = 0; i < attack[frames- frameLeft].Count; i++)
        {
            try
            {

                //Map.currentRoom.dammages.Add(initialPosition + attack[frames - frameLeft][i], new Attack(this, attack[frames - frameLeft][i]));
                Map.currentRoom.map[kofl.vectorInt(initialPosition + attack[frames - frameLeft][i])].attacks.Add(new Attack(this, attack[frames - frameLeft][i]));
                if (whom == 0)
                {
                    Map.currentRoom.map[kofl.vectorInt(initialPosition + attack[frames - frameLeft][i])].ennemiesamo = true;
                }
                var tempObject = GameObject.Instantiate(GameObject.Find("Cube"), (Vector2)initialPosition + attack[frames - frameLeft][i], Quaternion.identity, null);
                tempObject.GetComponent<SpriteRenderer>().sprite = weapon.sprite;
                tempAnim.Add(tempObject);
                temp.Add(kofl.vectorInt(initialPosition + attack[frames - frameLeft][i]));
            
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
           
        }
        frameLeft -= 1;
        if (frameLeft == -1)
        {
            return true;
        }
        return false;
    }
}