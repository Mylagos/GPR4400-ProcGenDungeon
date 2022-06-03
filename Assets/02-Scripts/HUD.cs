using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    //
    [SerializeField] private PlayerMovement _player;
    private TextMeshProUGUI _healthBarText;
    [SerializeField] private GameObject _healthMask;
    [SerializeField] private GameObject _healthBar;
    private float _maskPositionEmpty = -240f;

    [SerializeField] private Sprite _attackPinOn;
    [SerializeField] private Sprite _attackPinOff;
    [SerializeField] private Sprite _movementPinOn;
    [SerializeField] private Sprite _movementPinOff;

    [SerializeField] private GameObject _pinAtk;
    [SerializeField] private GameObject _pinMov1;
    [SerializeField] private GameObject _pinMov2;
    [SerializeField] private GameObject _pinMov3;
    [SerializeField] private GameObject _pinMov4;


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
        SetPlayerPins();
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

    private void SetPlayerPins()
    {
        int playerMoves = _player.moves;
        //int playerAttack = _player.attack;

        _pinAtk.transform.GetComponent<Image>().sprite = _attackPinOff;
        _pinMov1.GetComponent<Image>().sprite = _movementPinOff;
        _pinMov2.GetComponent<Image>().sprite = _movementPinOff;
        _pinMov3.GetComponent<Image>().sprite = _movementPinOff;
        _pinMov4.GetComponent<Image>().sprite = _movementPinOff;

        if ((0 == 0))
        {
            _pinAtk.SetActive(true);
        }

        if (playerMoves >= 0)
        {
            _pinMov1.SetActive(true);
        }
        if (playerMoves >= 1)
        {
            _pinMov2.SetActive(true);
        }
        if (playerMoves >= 2)
        {
            _pinMov3.SetActive(true);
        }
        if (playerMoves >= 3)
        {
            _pinMov4.SetActive(true);
        }
    }
}
