using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Header("Objects")]
    public GameObject LaserPrefab;
    public GameObject Background;
    public GameObject EngineFire;
    public ShipData SpaceShipData;

    [Header("Speed")]
    public float RotateSpeed;
    public float MaxSpeed;

    private Rigidbody2D _rigidbody2d;
    private Transform _transform;
    private Material _material;
    private SoundManager _soundManager;
    private Pool _laserPool;

    private bool _forward = false;
    private bool _brake = false;
    private float _rotate = 0;
    private float _reload;

    /// <param name="velocity">Ускорение</param>
    public delegate void MoveHandler(Vector2 velocity);

    /// <summary>
    /// События движения игрока
    /// </summary>
    public static event MoveHandler OnMove;

    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _transform = transform;
        _reload = SpaceShipData.ReloadTime;
        _laserPool = FindObjectOfType<PoolManager>().GetPool(PoolType.Laser);

        GetComponent<SpriteRenderer>().sprite = SpaceShipData.ShipSprite;

        _material = Background.GetComponent<MeshRenderer>().material;
        _soundManager = GetComponent<SoundManager>();

        EngineFire.transform.localPosition = SpaceShipData.FirePosition;
    }

    private void Update()
    {
        RotateKeyHandler();
        MoveKeyHandler();

        if (Input.GetKey(KeyCode.Space)) _brake = true;
        else _brake = false;

        if (Input.GetMouseButton(0) && _reload >= SpaceShipData.ReloadTime) Shoot();

        if (_reload < SpaceShipData.ReloadTime)
        {
            _reload += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();

        _material.mainTextureOffset += _rigidbody2d.velocity / 1000;
    }

    private void RotateKeyHandler()
    {
        if (Input.GetKey(KeyCode.A)) _rotate = 1;
        else if (Input.GetKey(KeyCode.D)) _rotate = -1;
        else _rotate = 0;
    }

    private void Rotate()
    {
        if (_rotate != 0)
        {
            _rigidbody2d.angularVelocity = 0;
            _rigidbody2d.rotation += (RotateSpeed * 10 * Time.fixedDeltaTime * _rotate);
        }
    }

    private void MoveKeyHandler()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            _forward = true;

            if (!_soundManager.IsPlaying("Engine"))
                _soundManager.Play("Engine");

            if (!EngineFire.activeInHierarchy)
                EngineFire.SetActive(true);
        }
        else
        {
            _forward = false;

            if (_soundManager.IsPlaying("Engine"))
                _soundManager.Stop("Engine");

            if (EngineFire.activeInHierarchy)
                EngineFire.SetActive(false);
        }
    }

    private void Move()
    {
        if (_brake && _rigidbody2d.velocity != Vector2.zero)
        {
            _rigidbody2d.AddForce(Braking() * Time.deltaTime * SpaceShipData.Speed, ForceMode2D.Impulse);
            OnMove?.Invoke(_rigidbody2d.velocity);
        }
        else if (_forward)
        {
            Vector2 velocity = _rigidbody2d.velocity + (Vector2)_transform.up * Time.fixedDeltaTime * SpaceShipData.Speed;

            if (velocity.magnitude <= MaxSpeed)
                _rigidbody2d.AddForce(_transform.up * Time.fixedDeltaTime * SpaceShipData.Speed, ForceMode2D.Impulse);

            OnMove?.Invoke(_rigidbody2d.velocity);
        }
    }

    private void Shoot()
    {
        _soundManager.Play("Shoot");
        _reload = 0;

        GameObject laser = _laserPool.GetPooledObject();
        Transform laserTransform = laser.transform;

        laserTransform.position = _transform.position;
        laserTransform.transform.rotation = _transform.rotation;
        laser.SetActive(true);
    }

    private Vector2 Braking()
    {
        Vector2 velocity = _rigidbody2d.velocity.normalized;

        if (velocity.x != 0)
            velocity.x /= Mathf.Abs(velocity.x);

        if (velocity.y != 0)
            velocity.y /= Mathf.Abs(velocity.y);

        return -velocity;
    }
}
