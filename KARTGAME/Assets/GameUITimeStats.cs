using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameUITimeStats : MonoBehaviour
{
    [SerializeField] Text text;

    void OnEnable() {
        TrackManager.PlayerGhostTime += UpdateTimes;
    }

    void OnDisable() {
        TrackManager.PlayerGhostTime -= UpdateTimes;
    }

    void UpdateTimes(float playerTime, float ghostTime) {
        text.text = "Player Time: " + TimeToString(playerTime) + "\n\n" + "Ghost Time: " + TimeToString(ghostTime);
    }


    string TimeToString(float time) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return timeSpan.ToString(@"mm\:ss\:ff");
    }
}
