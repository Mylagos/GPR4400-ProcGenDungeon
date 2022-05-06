using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public GameObject Father;
    public Gradient color;
    [SerializeField]
    public bool chid = false;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (chid)
            {
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
