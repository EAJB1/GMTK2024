using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioSource source;
    public AudioMixerGroup mixerGroup;

    public bool loop;
    public float volume;
    public float pitchVariation;
}
