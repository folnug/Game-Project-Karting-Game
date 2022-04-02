using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    //Aloitusarvo
    float currentTime = 0, countdown = 4;

    public Text currentTimeText, finalTimeText, countdownText;
    public Button rePlayButton;
    KartController[] controllers;

    //Audio
    public AudioSource countdownAudioSource;
    public AudioClip[] countdownAudioList;
    public float countdownAudioVolume;

    public AudioSource backgroundAudioSource;
    public AudioClip[] backgroundAudioList;
    public float backgroundAudioVolume;

    private bool maalissa = false;

    void Start()
    {
        controllers = FindObjectsOfType<KartController>();
        ToggleKartController(false);
        
        
        //SoundController.PlayAudio(backgroundAudioSource, backgroundAudioList, backgroundAudioVolume);

    }
    void ToggleKartController(bool toggle) {
        foreach(KartController controller in controllers) controller.enabled = toggle;
    }
    void Update()
    {
        StartCountdownAudio();
        StartCountdown();
    }

    private void StartCountdownAudio()
    {
        if (!countdownAudioSource.isPlaying)
        {
            for (int i = 2; i < countdown; i++)
                SoundController.PlayAudio(countdownAudioSource, countdownAudioList, countdownAudioVolume);
        }
    }

    private void StartCountdown()
    {
        countdown -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(countdown);
        countdownText.text = timeSpan.ToString("%s");

        if (maalissa)
            return;

        if (countdown < 1)
        {  
            //T�ss� kello k�y koko ajan
            currentTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.ToString(@"mm\:ss\:ff");
            finalTimeText.text = time.ToString(@"mm\:ss\:ff");
            countdownText.gameObject.SetActive(false);
            ToggleKartController(true);
        }
    }

    //Kun saa viestin Goalista t�� pys�ytt�� timerin
    void GoalUpdate()
    {
        maalissa = true;
        currentTimeText.color = Color.green;
        finalTimeText.gameObject.SetActive(maalissa);
        finalTimeText.color = Color.green;
        rePlayButton.gameObject.SetActive(maalissa);
    }
}

