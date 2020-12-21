using System.Collections;
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

        if (!_isReload && _isFire) Shoot();
    }

    private void Shoot()
    {
        _soundManager.Play("Shoot");

        GameObject laser = _laserPool.GetPooledObject();

        if (laser == null) return;

        Transform laserTransform = laser.transform;

        laserTransform.position = _transform.position;
        laserTransform.transform.rotation = _transform.rotation;
        
        laser.GetComponent<Laser>().OwnerTag = _gameObject.tag;
        laser.SetActive(true);

        _isReload = true;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_spaceShipData.ReloadTime);
        _isReload = false;
    }
}
