using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerScript : MonoBehaviour
{
    private PlayerInput _input;
    [SerializeField]
    private GameObject _point;

    [SerializeField] private int _speed;

    private void Update()
    {
        var step = _speed * Time.deltaTime;
        if (_input.actions["Up"].triggered)
            moveUp();
        this.transform.position = Vector3.MoveTowards(transform.position, _point.transform.position, step);
        _point.transform.position = Vector3.MoveTowards(_point.transform.position, transform.position, step);


    }

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        //_point = this.gameObject.transform.GetChild(0).gameObject;
    }

    private void moveUp()
    {
        if (this.transform.position == _point.transform.position)
        {
            _point.transform.localPosition = (Vector3.up);
        }
    }

}
