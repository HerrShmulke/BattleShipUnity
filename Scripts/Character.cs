using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject Background = null;
    [SerializeField] private GameObject EngineFire = null;
    [SerializeField] private ShipData SpaceShipData = null;

    [Header("Speed")]
    [SerializeField] private float RotateSpeed = 0;
    [SerializeField] private float MaxSpeed = 0;

    private Rigidbody2D _rigidbody2d;
    private Transform _transform;
    private Material _material;
    private SoundManager _soundManager;
    private Pool _laserPool;

    private int _advance = 0;
    private int _surf = 0;
    private int _rotation = 0;
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
        _soundManager.Play("Engine");

    }

    private void Update()
    {
        RotateKeyListener();
        MoveKeyListener();

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

    private void RotateKeyListener()
    {
        if (Input.GetKey(KeyCode.Q)) _rotation = 1;
        else if (Input.GetKey(KeyCode.E)) _rotation = -1;
        else _rotation = 0;
    }

    private void Rotate()
    {
        if (_rotation != 0)
        {
            _rigidbody2d.angularVelocity = 0;
            _rigidbody2d.rotation += (RotateSpeed * 10 * Time.fixedDeltaTime * _rotation);
        }
    }

    private void MoveKeyListener()
    {
        if (Input.GetKey(KeyCode.W)) _advance = 1;
        else if (Input.GetKey(KeyCode.S)) _advance = -1;
        else _advance = 0;

        if (Input.GetKey(KeyCode.A)) _rotation = 1;
        else if (Input.GetKey(KeyCode.D)) _rotation = -1;
        else _rotation = 0;

        if (Input.GetKey(KeyCode.Q)) _surf = -1;
        else if (Input.GetKey(KeyCode.E)) _surf = 1;
        else _surf = 0;
    }

    private void Move()
    {
        if (_advance != 0 || _surf != 0)
        {
            _rigidbody2d.AddForce(Time.fixedDeltaTime * _transform.up * _advance * SpaceShipData.Speed, ForceMode2D.Impulse);
            _rigidbody2d.AddForce(Time.fixedDeltaTime * _transform.right * _surf * SpaceShipData.Speed, ForceMode2D.Impulse);

            if (_soundManager.GetVolume("Engine") != 0.2f)
                _soundManager.SetVolume("Engine", 0.2f);

            if (!EngineFire.activeInHierarchy)
                EngineFire.SetActive(true);
        }
        else if (_advance == 0 && _surf == 0)
        {
            _rigidbody2d.AddForce(-_rigidbody2d.velocity * Time.fixedDeltaTime * SpaceShipData.Speed, ForceMode2D.Impulse);
            if (_soundManager.GetVolume("Engine") != 0.05f)
                _soundManager.SetVolume("Engine", 0.05f);

            if (EngineFire.activeInHierarchy)
                EngineFire.SetActive(false);
        }

        _rigidbody2d.angularVelocity = 0;
        _rigidbody2d.velocity = Vector2.ClampMagnitude(_rigidbody2d.velocity, MaxSpeed);

        OnMove?.Invoke(_rigidbody2d.velocity);
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
}
