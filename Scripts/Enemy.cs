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

    private float _radius = 10;
    private float _angle = 0;

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
            float angle = Vector3.SignedAngle(-_moveDirection, _transform.up, Vector3.forward);
            float rotateAngle = angle / Mathf.Abs(angle) * 10 * RotateSpeed * Time.deltaTime;

            angle = Mathf.Round(angle);

            if (Mathf.Abs(angle) < 179)
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
        float distance = Vector3.Distance(_playerTransform.position, _transform.position);

        _angle += Time.fixedDeltaTime;

        if (distance < _radius)
        {
            _rigidbody2D.AddForce(_moveDirection.normalized * Time.fixedDeltaTime * SpaceShipData.Speed, ForceMode2D.Impulse);
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, MaxSpeed);
        }
        else
        {
            float x = Mathf.Cos(_angle) * _radius + Player.transform.position.x;
            float y = Mathf.Sin(_angle) * _radius + Player.transform.position.y;

            Vector2 newDirection = new Vector2(x, y) - (Vector2)transform.position;

            _rigidbody2D.AddForce(newDirection.normalized * Time.fixedDeltaTime * SpaceShipData.Speed, ForceMode2D.Impulse);
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, MaxSpeed);
        }
    }
}
