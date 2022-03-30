using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    float currentTime;
    float countdown;
    public Text currentTimeText;
    public Text finalTimeText;
    public Text countdownText;
    public Button rePlayButton;
    private bool maalissa = false;
    private bool started = false;

    void Start()
    {
        //Aloitusarvo
        currentTime = 0;
        countdown = 4;
    }

    void Update()
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

