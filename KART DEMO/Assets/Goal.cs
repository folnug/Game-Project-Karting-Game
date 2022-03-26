using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Goal : MonoBehaviour
{
    private bool isTriggered = false;

    public Text currentLapText;
    float currentLap;
    float canIFinish;

    void Start()
    {
        //Aloitusarvot
        currentLap = 1;
        canIFinish = 0;
    }

    //Objekti jolla on rigidbody osuu t‰h‰n
    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered)
            return;

        //Kuinka monta checkpointtia on ja jos tarpeeksi monen checkpointin l‰pi on menty l‰hett‰‰ t‰‰ viestin mik‰ resettaa kaikki checkpointit
        //ja lap counter nousee ja resettaa l‰pikuljettujen checkpointtien m‰‰rn nollaan
        if (canIFinish == 2)
        {
            Debug.Log("Auto tuli maaliin");
            BroadcastMessage("CheckpointReset");
            currentLap++;
            currentLapText.text = "Lap " + currentLap.ToString() + " / 3";
            canIFinish = 0;
        }

        //Kun viimeinen kierros on ajettu l‰hett‰‰ t‰‰ viestin timerille nimell‰ GoalUpdate et se pys‰htyy (esim. jos max kierros on 3 k‰yt‰ 4)
        if (currentLap == 4)
        {
            GameObject.Find("Kart").SendMessage("GoalUpdate");
        }

        if (currentLap == 3)
        {
            GameObject.Find("BG Music").SendMessage("Faster");
        }
    }

    //Checkpointit l‰hett‰‰ t‰n viestin Goalille
    //L‰pi ajettujen checkpointtien m‰‰r‰ nousee joka kerta kun checkpointin l‰pi ajaa
    void Checkpoint()
    {
        canIFinish++;
        Debug.Log(canIFinish);
    }

    //Vastaanottaa ottaa CheckpointReset viestin
    //resettaa checkpointit ja goalin
    void CheckpointReset()
    {
        isTriggered = false;
        Debug.Log("Sain reset viestin");
    }
}

