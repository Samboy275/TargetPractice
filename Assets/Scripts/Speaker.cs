using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{

    // singleton
    public static Speaker instance { get; private set;}
    // components
    private AudioSource gameMusic;

    // control variables
    private bool isPlaying;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        gameMusic = GetComponent<AudioSource>();
        isPlaying = false;
    }

    public void PlayMusic()
    {
        gameMusic.Play();
    }

    public void TrunMusicOnOff()
    {
        if (isPlaying)
        {
            isPlaying = false;
            gameMusic.Stop();
        }
        else
        {
            isPlaying = true;
            gameMusic.Play();
        }
    }
}
