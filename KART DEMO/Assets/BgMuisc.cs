using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMuisc : MonoBehaviour
{
    private AudioSource audioSource;

    void Faster()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1.5f;
    }
}
