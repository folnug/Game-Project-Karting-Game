using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    float currentTime;
    public Text currentTimeText;
    public Text finalTimeText;
    private bool maalissa = false;

    void Start()
    {
        //Aloitusarvo
        currentTime = 0;
    }

    void Update()
    {
        if (maalissa)
            return;

        //T‰ss‰ kello k‰y koko ajan
        currentTime = currentTime + Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:ff");
        finalTimeText.text = time.ToString(@"mm\:ss\:ff");
    }

    //Kun saa viestin Goalista t‰‰ pys‰ytt‰‰ timerin
    void GoalUpdate()
    {
        maalissa = true;
        currentTimeText.color = Color.green;
        finalTimeText.gameObject.SetActive(maalissa);
        finalTimeText.color = Color.green;
    }
}

