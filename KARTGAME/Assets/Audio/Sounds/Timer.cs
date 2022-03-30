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
    KartController kc;

    private bool maalissa = false, started = false;

    void Start()
    {
        kc = GetComponent<KartController>();
        DisableKartController();
    }
    void DisableKartController() => kc.enabled = false;

    void EnableKartController() => kc.enabled = true;
    void Update()
    {
        StartCountdown();
    }

    private void StartCountdown()
    {
        countdown -= Time.deltaTime;
        TimeSpan countdowntime = TimeSpan.FromSeconds(countdown);
        countdownText.text = countdowntime.ToString("ss");

        if (maalissa)
            return;

        if (countdown < 1)
        {
            //Tässä kello käy koko ajan
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.ToString(@"mm\:ss\:ff");
            finalTimeText.text = time.ToString(@"mm\:ss\:ff");
            countdownText.gameObject.SetActive(false);
            EnableKartController();
        }
    }

    //Kun saa viestin Goalista tää pysäyttää timerin
    void GoalUpdate()
    {
        maalissa = true;
        currentTimeText.color = Color.green;
        finalTimeText.gameObject.SetActive(maalissa);
        finalTimeText.color = Color.green;
        rePlayButton.gameObject.SetActive(maalissa);
    }
}

