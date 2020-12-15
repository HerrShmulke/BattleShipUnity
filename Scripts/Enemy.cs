using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Objects")]
    public GameObject LaserPrefab;
    public GameObject Player;
    public ShipData SpaceShipData;

    [Header("Speed")]
    public float RotateSpeed;
    public float MaxSpeed;

    private Transform _playerTransform;
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    private Vector3 _moveDirection;

    private float _radius = 6;
    private float _angle = 0;
    private float _playerAngle = 0;
    private float _distance = 0;

    void Start()
    {
        _playerTransform = Player.transform;
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _moveDirection = _playerTransform.position - _transform.position;

        if (_moveDirection != Vector3.zero)
        {
            _playerAngle = Vector3.SignedAngle(-_moveDirection, _transform.up, Vector3.forward);
            float rotateAngle = _playerAngle / Mathf.Abs(_playerAngle) * 10 * RotateSpeed * Time.deltaTime;

            _playerAngle = Mathf.Round(_playerAngle);

            if (Mathf.Abs(_playerAngle) < 179)
            {
                _rigidbody2D.rotation += rotateAngle;
                _rigidbody2D.angularVelocity = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _distance = Vector3.Distance(_playerTransform.position, _transform.position);

        if (_distance > 20)
        {
            _transform.position = _playerTransform.position + new Vector3(8, 8, 0);
            _rigidbody2D.velocity = new Vector2(0, 0);
            _rigidbody2D.rotation = _playerAngle;

            return;
        }

        _angle += Time.fixedDeltaTime;

        Vector2 direction = _moveDirection;
        float tempMaxSpeed = MaxSpeed;

        if (_distance <= _radius)
        {
            float x = Mathf.Cos(_angle) * _radius + Player.transform.position.x;
            float y = Mathf.Sin(_angle) * _radius + Player.transform.position.y;

            direction = new Vector2(x, y) - (Vector2)transform.position;
        }

        _rigidbody2D.AddForce(direction.normalized * Time.fixedDeltaTime * SpaceShipData.Speed, ForceMode2D.Impulse);
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, tempMaxSpeed);
    }
}
