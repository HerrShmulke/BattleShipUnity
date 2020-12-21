using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Laser : MonoBehaviour
{
    public LaserData laserData;
    public string OwnerTag;


    private SpriteRenderer _spriteRenderer;
    private int _check = 0;

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * laserData.Speed, ForceMode2D.Impulse);
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = laserData.LaserSprite;
    }

    private void Update()
    {
        if (_check == 50)
        {
            if (!_spriteRenderer.isVisible)
            {
                gameObject.SetActive(false);
            }
            
            _check = 0;
        }
        else
        {
            ++_check;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == OwnerTag)
        {
            return;
        }
        else if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Health -= laserData.Damage;
        }
    }
}
