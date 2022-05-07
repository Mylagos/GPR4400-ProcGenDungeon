using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //the ennemies to whom the bar is attached
    public GameObject Father;
    //the color of the bar depending on the health
    public Gradient color;
    //if it is a copy
    public bool child = false;


    void Update()
    {
        try
        {
            if (child)
            {
                //follow the ennemies and adjust it value and color dependin on its health
                transform.position = Father.transform.GetChild(1).transform.position - new Vector3(0, 1);
                var temp = Father.GetComponent<EnnemieBehaviour>();
                GetComponent<Slider>().value = (float)(temp.currentLife / temp.life);
                transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = color.Evaluate(temp.currentLife / temp.life);
            }
           
        }
        catch {
            gameObject.SetActive(false);
        }
       


    }
}
