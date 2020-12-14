using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Objects")]
    public GameObject LaserPrefab;
    public GameObject EngineFire;
    public GameObject Player;
    public GameObject TargetPoint;
    public ShipData SpaceShipData;

    [Header("Speed")]
    public float RotateSpeed;
    public float MaxSpeed;

    private Transform _playerTransform;
    private Rigidbody2D _playerRigidbody2D;
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _playerTransform = Player.transform;
        _playerRigidbody2D = Player.GetComponent<Rigidbody2D>();
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 moveDirection = _playerTransform.position - _transform.position;

        if (moveDirection != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(-moveDirection, _transform.up, Vector3.forward);
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
        float tempMaxSpeed = MaxSpeed;

        if (distance < 3)
            tempMaxSpeed -= 1;
        else if (distance < 6)
            tempMaxSpeed = MaxSpeed;
        else if (distance < 15)
            tempMaxSpeed += 0.4f;
        else
            tempMaxSpeed += 1.5f;
        

        _rigidbody2D.velocity = _transform.up * Time.fixedDeltaTime * tempMaxSpeed * 100;
    }
}
