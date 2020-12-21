using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipData", menuName = "Ship Data", order = 51)]
public class ShipData : ScriptableObject
{
    public Sprite ShipSprite;
    public Vector2 FirePosition;

    public float Speed;
    public float ReloadTime;
    public float MaxHealth;
}
