using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostpad : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float impulseBoostAmount;

    //Audio
    public AudioSource boostpadAudioSource;
    public AudioClip[] boostpadAudioList;
    public float boostpadAudioVolume;

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag != "Kart") return;

        KartController kc = other.transform.GetComponent<KartMotor>().kartController;
        kc.AddBoostTime(time);
        kc.GiveImpulseBoost(impulseBoostAmount);
        SoundController.PlayAudio(boostpadAudioSource, boostpadAudioList, boostpadAudioVolume);
    }
}
