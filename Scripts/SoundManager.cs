using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public Sound[] Sounds;

    private Dictionary<string, Sound> _sounds;

    private void Awake()
    {
        _sounds = new Dictionary<string, Sound>();

        for (int i = 0; i < Sounds.Length; ++i)
        {
            Sounds[i].source = gameObject.AddComponent<AudioSource>();
            Sounds[i].source.clip = Sounds[i].Clip;
            Sounds[i].source.volume = Sounds[i].Volume;
            Sounds[i].source.pitch = Sounds[i].Pitch;
            Sounds[i].source.loop = Sounds[i].Loop;
            Sounds[i].source.playOnAwake = false;

            _sounds[Sounds[i].Name.ToUpper()] = Sounds[i];
        }
    }

    public void Play(string name)
    {
        _sounds[name.ToUpper()].source.Play();
    }

    public bool IsPlaying(string name)
    {
        return _sounds[name.ToUpper()].source.isPlaying;
    }

    public void Stop(string name)
    {
        _sounds[name.ToUpper()].source.Stop();
    }
}
