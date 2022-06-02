using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    //
    [SerializeField] private PlayerMovement _player;
    private TextMeshProUGUI _healthBarText;
    [SerializeField] private GameObject _healthMask;
    [SerializeField] private GameObject _healthBar;
    private float _maskPositionEmpty = -240f;


    void Start()
    {
        _healthBarText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Print out the health text
        _healthBarText.text = _player.CurrentHealth.ToString() +"/"+_player.MaxHealth.ToString();
        HealthMaskOffset();
        Debug.Log("ooo");
    }

    private void HealthMaskOffset()
    {
        _healthMask.transform.localPosition = new Vector3(0.3f, 0, 0); 
        _healthBar.transform.localPosition = new Vector3(0.3f, 0, 0);
        float temp = (_maskPositionEmpty * _player.CurrentHealth / _player.MaxHealth);
        Vector3 tempVec = _healthMask.transform.localPosition;
        tempVec.x = (_maskPositionEmpty -  temp +0.3f);
        _healthMask.transform.localPosition = tempVec;
        _healthBar.transform.localPosition = new Vector3(-_maskPositionEmpty + temp, 0, 0);
    }
}
