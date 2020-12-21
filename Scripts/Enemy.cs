using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : SpaceShip
{
    [SerializeField] private GameObject _player = null;

    private Transform _playerTransform;

    private float _attackRadius = 3;
    private float _playerAngle = 0;
    private float _distance = 0;

    private Vector3 _oldMoveDirection;
    private Vector3 _moveDirection;

    protected override void Start()
    {
        base.Start();

        _playerTransform = _player.transform;
    }

    protected override void Update()
    {
        base.Update();

        Rotate();
        Move();
    }

    private void Rotate()
    {
        _moveDirection = _playerTransform.position - _transform.position;
        _playerAngle = Vector3.SignedAngle(-_moveDirection, _transform.up, Vector3.forward);

        if (Mathf.Abs(Mathf.Round(_playerAngle)) < 179)
        {
            _rotate = _playerAngle < 0 ? -1 : 1;
            _rigidbody2D.angularVelocity = 0;
        }
        else
        {
            _rotate = 0;
        }
    }
    
    private void Move()
    {
        _distance = Vector3.Distance(_playerTransform.position, _transform.position);

        if (Mathf.Abs(Mathf.Round(_playerAngle)) >= 179 && _distance > _attackRadius)
        {
            MoveVector = new Vector2(0, 1);
        }
        else
        {
            MoveVector = Vector2.zero;
        }
    }
}
