using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SoundManager))]
public class SpaceShip : MonoBehaviour
{
    [SerializeField] protected ShipData _spaceShipData = null;
    [SerializeField] private GameObject _engineFire = null;

    [Header("Restrictions")]
    [SerializeField] private float _rotateSpeed = 0;
    [SerializeField] private float _maxSpeed = 0;

    protected Rigidbody2D _rigidbody2D;
    protected Transform _transform;
    protected SoundManager _soundManager;

    protected float _rotate;

    private Vector2 _moveVector;

    public Vector2 MoveVector
    {
        get => _moveVector;

        set
        {
            if (value == Vector2.zero)
            {
                _engineFire.SetActive(false);
                _soundManager.SetVolume("Engine", 0.05f);
            }
            else
            {
                _engineFire.SetActive(true);
                _soundManager.SetVolume("Engine", 0.2f);
            }

            _moveVector = value;
        }
    }

    protected virtual void Start()
    {
        _transform = transform;
        _moveVector = Vector2.zero;
        _soundManager = GetComponent<SoundManager>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _engineFire.SetActive(false);
        _engineFire.transform.localPosition = _spaceShipData.FirePosition;
        _soundManager.Play("Engine");
        _soundManager.SetVolume("Engine", 0.05f);
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (_moveVector != Vector2.zero) Move();
        else if (_rigidbody2D.velocity != Vector2.zero) Brake();

        if (_rotate != 0) Rotate();
    }

    private void Move()
    {
        _rigidbody2D.AddForce(Time.fixedDeltaTime * _moveVector.y * transform.up * _spaceShipData.Speed, ForceMode2D.Impulse);
        _rigidbody2D.AddForce(Time.fixedDeltaTime * _moveVector.x * transform.right * _spaceShipData.Speed, ForceMode2D.Impulse);
        _rigidbody2D.angularVelocity = 0;
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }

    private void Brake()
    {
        _rigidbody2D.AddForce(-_rigidbody2D.velocity * Time.fixedDeltaTime * _spaceShipData.Speed, ForceMode2D.Impulse);
    }

    private void Rotate()
    {
        _rigidbody2D.rotation += _rotateSpeed * 10 * Time.fixedDeltaTime * _rotate;
    }
}
