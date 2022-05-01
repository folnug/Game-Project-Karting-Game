using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] Text timer;
    [SerializeField] Text laps;
    [SerializeField] Text position;
    [SerializeField] Text counter;
    [SerializeField] Slider boostbar;
    Transform selectedKart;
    CheckpointHandler checkpointHandler;
    KartController selectedController;
    KartCheckpointData selectedData;

    void Awake() {
        boostbar.value = 0f;
        boostbar.maxValue = 100f;
    }

    void OnEnable() {
        StartCountdown.CountdownUpdate += UpdateCountdown;
        StartCountdown.CountdownComplete += CountdownCompleted;
        CheckpointHandler.TimerUpdate += UpdateTimer;
        TrackManager.SelectedKart += SelectKart;
    }

    void OnDisable() {
        StartCountdown.CountdownUpdate -= UpdateCountdown;
        StartCountdown.CountdownComplete -= CountdownCompleted;
        CheckpointHandler.TimerUpdate -= UpdateTimer;
        TrackManager.SelectedKart -= SelectKart;
    }

    void UpdateCountdown(float time) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time + 1);
        counter.text = timeSpan.ToString("%s");
    }

    void UpdateTimer(float time) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timer.text = timeSpan.ToString(@"mm\:ss\:ff");
    }

    void CountdownCompleted() {
        counter.gameObject.SetActive(false);
    }

    void SelectKart(KartController kart) {
        selectedController = kart;
        selectedData = kart.transform.GetComponent<KartCheckpointData>();

        if (selectedData == null || selectedController == null) return;
    }

    void Update() {
        if (selectedData == null || selectedController == null) return;
        boostbar.value = selectedController.driftValue;
        laps.text = (selectedData.laps + 1) + " / " + selectedData.maxLaps;
        position.text = selectedData.position + PositionEnding(selectedData.position);
    }

    string PositionEnding(int position) {
        switch(position) {
            case 1:
                return "st";
            case 2:
                return "nd";
            case 3:
                return "rd";
            default:
                return "th";
        }
    }
}
