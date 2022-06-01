using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update

   /* [SerializeField] private PlayerMovement _player;
    private TextMeshProUGUI _healthBarText;
    [SerializeField]
    private GameObject _healthMask;
    private float _maskPositionFull = -0.064f;
    private float _maskPositionEmpty = -1.702f;


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
    }

    private void HealthMaskOffset()
    {
        float temp = (_maskPositionFull * _player.CurrentHealth / _player.MaxHealth);
        Vector3 tempVec = _healthMask.transform.localPosition;
        tempVec.x = (_maskPositionEmpty + temp);
        _healthMask.transform.localPosition = tempVec;
    }*/
}
