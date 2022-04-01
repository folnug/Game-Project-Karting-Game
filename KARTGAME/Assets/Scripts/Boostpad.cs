using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostpad : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float impulseBoostAmount;

    //Audio
    public AudioSource audioSource;
    public AudioClip[] boostAudio;
    private int toPlay;
    
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag != "Kart") return;

        KartController kc = other.transform.GetComponent<KartMotor>().kartController;
        kc.AddBoostTime(time);
        kc.GiveImpulseBoost(impulseBoostAmount);
        PlayAudio(boostAudio);
    }

    private void PlayAudio(AudioClip[] boostAudio)
    {
        toPlay = Random.Range(0, boostAudio.Length);
        audioSource.PlayOneShot(boostAudio[toPlay], 0.2F);
        audioSource.Play();
    }
}
