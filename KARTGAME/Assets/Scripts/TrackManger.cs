<<<<<<< Updated upstream
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
>>>>>>> Stashed changes

public class TrackManger : MonoBehaviour
{
    [SerializeField] CharacterSelection characterSelection;
<<<<<<< Updated upstream
    [SerializeField] float duration = 0f;
    KartController[] kartControllers;
=======
>>>>>>> Stashed changes

    CheckpointHandler checkpointHandler;

    KartCheckpointData player;

    bool gameOver = false;

<<<<<<< Updated upstream
    public UnityEvent GameOver;
    public UnityEvent StartRace;
    void Awake() {
        kartControllers = FindObjectsOfType<KartController>();
=======
    void Awake() {
>>>>>>> Stashed changes
        checkpointHandler = GetComponent<CheckpointHandler>();
        player = checkpointHandler.GetPlayer();
    }

    void Update() {
<<<<<<< Updated upstream
        StartTimer();

=======
>>>>>>> Stashed changes
        if (player == null) {
            player = checkpointHandler.GetPlayer();
            return;
        }
<<<<<<< Updated upstream

        if (player.laps == characterSelection.maxlaps && !gameOver) {
            gameOver  = true;
            GameOver?.Invoke();
=======
        if (player.laps == characterSelection.maxlaps && !gameOver) {
            Debug.Log("Game over!");
            gameOver  = true;
>>>>>>> Stashed changes
            ChangePlayerController();
        }
    }

<<<<<<< Updated upstream
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

=======
>>>>>>> Stashed changes
    void ChangePlayerController() {
        Destroy(player.transform.GetComponent<PlayerInput>());
        player.gameObject.AddComponent<AiInput>();
    }
<<<<<<< Updated upstream
    public float GetCountdown() => duration;

    public string GetCountdownString() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        return timeSpan.ToString("%s");
    }
=======
>>>>>>> Stashed changes
}
