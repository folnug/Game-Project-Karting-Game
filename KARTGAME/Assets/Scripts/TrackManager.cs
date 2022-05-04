using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class TrackManager : MonoBehaviour
{

    [SerializeField] CharacterSelection characterSelection;
    public enum GameStates {
        Spawn,
        Intro,
        Countdown,
        Race,
        End,
    }

    public enum GameModes {
        TimeTrial = 0,
        Arcade = 1,
    }

    public GameStates currentState { get; private set; }
    GameStates lastState;

    GameModes currentGameMode;

    Transform kartInFirstPos;

    Ghost currentGhost;

    #region Events

    public static event Action<CharacterSelection> SpawnKarts;
    public static event Action<CharacterSelection> SpawnKart;
    public static event Action PlayIntro;
    public static event Action StartCounter;
    public static event Action SetupRace;
    public static event Action ReplayGhost;
    public static event Action<GameModes> EndRace;
    public static event Action<KartController> SelectedKart;

    public static event Action<Transform> WinnerKart;

    public static event Action<float, float> PlayerGhostTime;


    #endregion

    KartController[] karts;

    void Awake() {
        currentState = GameStates.Spawn;
        lastState = GameStates.Intro;
        currentGameMode = characterSelection.gameMode;
    }

    void OnEnable() {
        KartSpawner.KartSpawnComplete += KartSpawnComplete;
        StartCountdown.CountdownComplete += CounterComplete;
        CheckpointHandler.KartCompletedLap += KartCompletedLap;
        CheckpointHandler.KartInFirstPos += KartInFirstPosition;
        Player.GhostData += SetGhost;
    }

    void OnDisable() {
        KartSpawner.KartSpawnComplete -= KartSpawnComplete;
        StartCountdown.CountdownComplete -= CounterComplete;
        CheckpointHandler.KartCompletedLap -= KartCompletedLap;
        CheckpointHandler.KartInFirstPos -= KartInFirstPosition;
        Player.GhostData -= SetGhost;
    }

    void Update() {
        if (currentState != lastState) UpdateTrackManager();
    }
    
    void UpdateTrackManager() {
        lastState = currentState;
        Debug.Log(currentState);

        switch(currentState) {
            case GameStates.Spawn:
                Spawn();
                break;
            case GameStates.Intro:
                PlayIntro?.Invoke();
                break;
            case GameStates.Countdown:
                //SoundManager.PlaySoundCountdown(SoundManager.Sound.TrackCountdown, 0.05f);
                StartCounter?.Invoke();
                break;
            case GameStates.Race:
                Race();
                break;
            case GameStates.End:
                End();
                break;
        }
    }
    void Race() {
        if (currentGameMode == GameModes.TimeTrial) ReplayGhost?.Invoke();
        SetupRace?.Invoke();
        EnableKarts();
        SoundManager.EnableBGMusic();
    }
    void Spawn() {
        switch(currentGameMode) {
            case GameModes.TimeTrial:
                SpawnKart?.Invoke(characterSelection);
                SelectedKart?.Invoke(karts[0]);
                break;
            case GameModes.Arcade:
                SpawnKarts?.Invoke(characterSelection);
                SelectedKart?.Invoke(karts[characterSelection.playerCharacterIndex]);
                break;
        }
    }

    void EnableKarts() {
        if (karts == null) return;
        foreach(KartController kartController in karts) {
            kartController.SetState(KartController.KartStates.Drive);
        }
    }

    void KartSpawnComplete(KartController[] karts) {
        this.karts = karts;
        currentState = GameStates.Countdown;
    }

    void CounterComplete() {
        currentState = GameStates.Race;
    }
    void KartCompletedLap(KartCheckpointData data) {
        PlayerInput player = data.transform.GetComponent<PlayerInput>();
        if (data.laps == characterSelection.maxlaps && player != null) {
            RaceComplete();
            // Swap Controller
            Destroy(player.transform.GetComponent<PlayerInput>());
            player.gameObject.AddComponent<AiInput>();
        }
    }

    void RaceComplete() {
        currentState = GameStates.End;
    }

    void KartInFirstPosition(KartCheckpointData data) {   
        kartInFirstPos = data.transform;
    }

    void SetGhost(Ghost ghost) {
        currentGhost = ghost;
    }

    void End() {
        EndRace?.Invoke(characterSelection.gameMode);
        if (currentGameMode == GameModes.TimeTrial) {
            bool playerWon = kartInFirstPos.GetComponent<KartCheckpointData>().totalTime < currentGhost.totalTime;
            WinnerKart?.Invoke(playerWon ? kartInFirstPos : null);
            PlayerGhostTime?.Invoke(kartInFirstPos.GetComponent<KartCheckpointData>().totalTime, currentGhost.totalTime);
            return;
        }
        WinnerKart?.Invoke(kartInFirstPos);
    }
}
/*
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

    public int GetMaxLaps() => characterSelection.maxlaps;

    public string GetCountdownString() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration + 1);
        return timeSpan.ToString("%s");
    }
}
*/