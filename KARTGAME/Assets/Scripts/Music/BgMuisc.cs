using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMuisc : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] backgroundMusicList;

    public void Start()
    {
        PlayTrackMusic(0);
    }

    public void PlayTrackMusic(int num)
    {
        audioSource.PlayOneShot(backgroundMusicList[num], 0.2F);
        audioSource.Play();
    }
}

