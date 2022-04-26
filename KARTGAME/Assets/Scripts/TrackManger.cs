using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackManger : MonoBehaviour
{
    [SerializeField] CharacterSelection characterSelection;
    [SerializeField] float duration = 0f;
    KartController[] kartControllers;

    CheckpointHandler checkpointHandler;

    KartCheckpointData player;

    bool gameOver = false;

    public UnityEvent GameOver;
    public UnityEvent StartRace;
    void Awake() {
        kartControllers = FindObjectsOfType<KartController>();
        checkpointHandler = GetComponent<CheckpointHandler>();
        player = checkpointHandler.GetPlayer();
    }

    void Update() {
        StartTimer();

        if (player == null) {
            player = checkpointHandler.GetPlayer();
            return;
        }

        if (player.laps == characterSelection.maxlaps && !gameOver) {
            gameOver  = true;
            GameOver?.Invoke();
            ChangePlayerController();
        }
    }

    void StartTimer() {
        if (duration == 0f) return;
        duration -= Time.deltaTime;
        if (duration <= 0f) {
            duration = 0f;
            kartControllers = FindObjectsOfType<KartController>();
            foreach(KartController kartController in kartControllers) kartController.SetState(KartController.KartStates.Drive);
            StartRace?.Invoke();
        }
    }

    void ChangePlayerController() {
        Destroy(player.transform.GetComponent<PlayerInput>());
        player.gameObject.AddComponent<AiInput>();
    }
    public float GetCountdown() => duration;

    public string GetCountdownString() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        return timeSpan.ToString("%s");
    }
}
