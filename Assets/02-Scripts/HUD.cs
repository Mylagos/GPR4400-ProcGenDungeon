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
        SetPlayerPrins();
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

    private void SetPlayerPrins()
    {
        int playerMoves = _player.moves;
        int playerAttack = _player.attack;

        _pinAtk.SetActive(false);
        _pinMov1.SetActive(false);
        _pinMov2.SetActive(false);
        _pinMov3.SetActive(false);
        _pinMov4.SetActive(false);

        if (!(playerAttack == 0))
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
