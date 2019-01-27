using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioSource backgroundSource;
    
    public AudioClip gameStart;
    public AudioClip playerReady;
    public AudioClip playerContact;
    public AudioClip playerLeap;
    public AudioClip playerRoll;
    public AudioClip playerVictory;

    public AudioClip gameMusic;

    void Start()
    {
        backgroundSource.loop = true;
        if (gameMusic != null)
        {
            backgroundSource.clip = gameMusic;
            backgroundSource.Play();
//            backgroundSource.PlayOneShot(gameMusic);
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void BackgroundMusic(bool play)
    {
        if (play)
        {
            backgroundSource.clip = gameMusic;
            backgroundSource.Play();
        }
        else
        {
            backgroundSource.Stop();
        }
    }
    
}
