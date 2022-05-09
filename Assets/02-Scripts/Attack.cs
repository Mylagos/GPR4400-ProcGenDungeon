using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public float dammage;
    public int whom;
    public int frameLeft;
    public int frames;
    public Vector2 initialPosition;
    public List<List<Vector2Int>> attack;
    public List<Vector2> temp = new List<Vector2>();
    public List<GameObject> tempAnim = new List<GameObject>();
    public Attack(List<List<Vector2Int>> attack, Vector2 initialPosition, float dammage, int whom)
    {
        this.attack = attack;
        frameLeft = attack.Count - 1;
        frames = attack.Count - 1;
        this.initialPosition = initialPosition;
        this.dammage = dammage;
        this.whom = whom;
    }
    public Attack(Attack attack, Vector2 recule)
    {
        initialPosition = recule;
        dammage = attack.dammage;
        whom = attack.whom;
    }
    public void destroyAt()
    {
        for (int i = 0; i < tempAnim.Count ; i++)
        {
            Map.currentRoom.dammages.Remove(temp[i]);
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
            if (!Map.currentRoom.dammages.ContainsKey(initialPosition + attack[frames - frameLeft][i]))
            {
                temp.Add(initialPosition + attack[frames - frameLeft][i]);
                Map.currentRoom.dammages.Add(initialPosition+ attack[frames - frameLeft][i], new Attack(this, attack[frames - frameLeft][i]));
                tempAnim.Add(GameObject.Instantiate(GameObject.Find("Cube"), (Vector2)initialPosition + attack[frames - frameLeft][i], Quaternion.identity, null));
            }
            else
            {
                Debug.Log("badremover");
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