using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : SpaceShip
{
    public UnityEvent<Vector2> PlayerMove;

    private Pool _laserPool;
    private bool _isReload = false;
    private bool _isFire = false;

    public void ListenRotateKey(InputAction.CallbackContext context)
    {
        _rotate = context.ReadValue<float>();
    }

    public void ListenMoveKey(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();
    }

    public void ListenShootKey(InputAction.CallbackContext context)
    {
        _isFire = context.ReadValue<float>() == 1f;
    }

    protected override void Start()
    {
        base.Start();

        _laserPool = FindObjectOfType<PoolManager>().GetPool(PoolType.Laser);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        PlayerMove.Invoke(_rigidbody2D.velocity);
    }

    protected override void Update()
    {
        base.Update();

        if (!_isReload && _isFire) Fire();
    }

    private void Fire()
    {
        _soundManager.Play("Shoot");
        _isReload = true;

        GameObject laser = _laserPool.GetPooledObject();
        Transform laserTransform = laser.transform;

        laserTransform.position = _transform.position;
        laserTransform.transform.rotation = _transform.rotation;
        laser.SetActive(true);

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_spaceShipData.ReloadTime);
        _isReload = false;
    }
}
