using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    [SerializeField] Text timer;
    [SerializeField] Text laps;
    [SerializeField] Text position;
    [SerializeField] Text counter;
    [SerializeField] Slider boostbar;

    CheckpointHandler checkpointHandler;
    KartController player;
    KartCheckpointData playerData;

    TrackManger trackManger;

    int lastLap = 1;
    int lastPosition = 0;

    void Awake() {
        checkpointHandler = FindObjectOfType<CheckpointHandler>();
        trackManger = FindObjectOfType<TrackManger>();
        player = FindObjectOfType<PlayerInput>().transform.GetComponent<KartController>();
        playerData = player.transform.GetComponent<KartCheckpointData>();
        boostbar.maxValue = 100f;
    }

    void Update() {

        if (checkpointHandler == null || player == null || playerData == null) return;

        if (trackManger.GetCountdown() > 0) {
            counter.text = trackManger.GetCountdownString();
            counter.gameObject.SetActive(true);
        } else {
            counter.gameObject.SetActive(false);
        }

        timer.text = checkpointHandler.GetTime();
        if (lastLap != playerData.laps) {
            laps.text = playerData.laps + "/" + trackManger.GetMaxLaps();
            lastLap = playerData.laps;
        }

        if (lastPosition != playerData.position) {
            position.text = playerData.position + PositionEnding(playerData.position);
            lastPosition = playerData.position;
        }

        boostbar.value = player.driftValue;
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
