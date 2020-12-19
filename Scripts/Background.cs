using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Background : MonoBehaviour
{
    private Material _material;

    public void PlayerMoveListener(Vector2 velocity)
    {
        _material.mainTextureOffset += velocity / 1000;
    }

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }
}
