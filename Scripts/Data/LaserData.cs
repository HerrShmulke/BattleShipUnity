using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LaserData", menuName = "Laser Data", order = 51)]
public class LaserData : ScriptableObject
{
    public Sprite LaserSprite;
    public float Speed;
    public int Damage;
}
