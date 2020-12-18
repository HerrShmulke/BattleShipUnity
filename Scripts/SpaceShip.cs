using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShip : MonoBehaviour
{
    [SerializeField] private GameObject EngineFire = null;
    [SerializeField] private ShipData SpaceShipData = null;

    [Header("Restrictions")]
    [SerializeField] private float RotateSpeed = 0;
    [SerializeField] private float MaxSpeed = 0;

    private Rigidbody2D _rigidbody2d;
    private Transform _transform;
    private Material _material;
    private SoundManager _soundManager;

    private int _forwardMotion = 0;
    private int _sideMotion = 0;
    private int _rotation = 0;
    private float _reload = 0;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public void Move()
    {

    }

    public void Rotate()
    {

    }
}
