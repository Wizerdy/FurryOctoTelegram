using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds = new Sound[0];
    public Sound music;

    private static SoundManager instance;
    public static SoundManager i
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        music.source = gameObject.AddComponent<AudioSource>();
        music.source.clip = music.clip;

        music.source.volume = music.volume;
        music.source.pitch = music.pitch;
        music.source.loop = music.loop;
    }

    public void Play(string name)
    {
        Sound s;
        s = name == "Music" ? music : Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("pas trouvé : " + name);
            return;
        }
        s.source.Play();
    }
}
