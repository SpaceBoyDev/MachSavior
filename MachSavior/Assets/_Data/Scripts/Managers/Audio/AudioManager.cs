using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Audio[] musicAudio, sfxAudio;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayMusic(string name)
    {
        Audio s = Array.Find(musicAudio, x => x.name == name);

        if (s == null)
        {
            print("canción " + name + " no encontrado");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Audio s = Array.Find(sfxAudio, x => x.name == name);
        print(name);
        if (s == null)
        {
            print("SFX " + name + " no encontrado");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
}
