using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
    public string Name;

    public AudioClip Clip;

    public bool Loop;

    [Range(0f, 1f)]
    public float Volume;

    [Range(.1f, 3f)]
    public float Pitch;

    [HideInInspector]
    public AudioSource source;
}
