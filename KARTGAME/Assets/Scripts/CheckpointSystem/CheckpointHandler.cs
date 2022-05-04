using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] Transform checkpointsTransform;
    List<Checkpoint> checkpoints = new List<Checkpoint>();
    float timer = 0f;

    bool runTimer = false;

    List<KartCheckpointData> kartCheckpointData = new List<KartCheckpointData>();

    public static event Action<KartCheckpointData> KartCompletedLap;
    public static event Action<float> TimerUpdate;

    public static event Action<KartCheckpointData> KartInFirstPos;

    KartCheckpointData firstPosKart;
    KartCheckpointData lastFirstPosKart;

    void OnEnable() {
        TrackManager.SetupRace += Init;
        TrackManager.EndRace += StopTimer;
    }

    void OnDisable() {
        TrackManager.SetupRace -= Init;
        TrackManager.EndRace -= StopTimer;
    }

    void Init() {
        KartCheckpointData[] tempKartCheckpointData = FindObjectsOfType<KartCheckpointData>();

        Checkpoint[] tempCheckpoints = checkpointsTransform.GetComponentsInChildren<Checkpoint>();
        foreach(Checkpoint checkpoint in tempCheckpoints) {
            checkpoints.Add(checkpoint);
            checkpoint.SetCheckpointHandler(this);
        }
        int i = 1;
        foreach(KartCheckpointData data in tempKartCheckpointData) {
            data.lastTimes.Add(0);
            data.nextCheckpoint = checkpoints[0];
            data.position = i;
            i += 1;
            kartCheckpointData.Add(data);
        }

        runTimer = true;

        firstPosKart = kartCheckpointData[0];
    }
    
    void FixedUpdate() {
        if (runTimer)
            timer += Time.deltaTime;
            TimerUpdate?.Invoke(timer);
        KartPosition();
        for (int i = 0; i < kartCheckpointData.Count; i++) {
            kartCheckpointData[i].position = i + 1;
        }
        firstPosKart = kartCheckpointData.Find(e => e.position == 1);
        if (firstPosKart != lastFirstPosKart) {
            lastFirstPosKart = firstPosKart;
            KartInFirstPos?.Invoke(firstPosKart);
        }

    }
    public void KartEnteredCheckpoint(KartCheckpointData data, Checkpoint checkpoint) {
        if (!runTimer) return;
        int nextCheckpoint = data.nextCheckpointIndex;
        if (checkpoints.IndexOf(checkpoint) == nextCheckpoint) {
            data.currentCheckpointIndex = nextCheckpoint;
            data.nextCheckpointIndex = nextCheckpoint + 1;
            data.CheckpointsCollected += 1;

            if (data.nextCheckpointIndex == checkpoints.Count) {
                data.times.Add(timer - data.lastTimes[data.laps]);
                data.lastTimes.Add(timer);
                
                data.laps += 1;
                data.nextCheckpointIndex = data.nextCheckpointIndex % checkpoints.Count;
                KartCompletedLap?.Invoke(data);
            }

            data.nextCheckpoint = checkpoints[data.nextCheckpointIndex];
        }
    }


    void KartPosition() {
        if (!runTimer) return;
        kartCheckpointData.Sort((kart1, kart2) => {
            if (kart1.CheckpointsCollected != kart2.CheckpointsCollected)
                return kart1.CheckpointsCollected.CompareTo(kart2.CheckpointsCollected);
            
            return kart2.DistanceToNext().CompareTo(kart1.DistanceToNext());
        });
        kartCheckpointData.Reverse();

    }

    public string GetTime() => TimeToString(timer);
    public KartCheckpointData GetPlayer() {
        foreach(KartCheckpointData kart in kartCheckpointData) {
            if (kart.transform.GetComponent<PlayerInput>() != null)
                return kart;
        }
        return null;
    }

    public void StartTimer() => runTimer = true;
    public void StopTimer(TrackManager.GameModes gameMode) => runTimer = false;
    
    public List<KartCheckpointData> GetKarts() => kartCheckpointData;

    string TimeToString(float currentTime) {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return time.ToString(@"mm\:ss\:ff");
    }
}
