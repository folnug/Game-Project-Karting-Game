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

    void Awake() {
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
    }
    
    void FixedUpdate() {
        if (runTimer)
            timer += Time.deltaTime;
        KartPosition();
        for (int i = 0; i < kartCheckpointData.Count; i++) {
            kartCheckpointData[i].position = i + 1;
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

    public KartCheckpointData GetPlayer() {
        foreach(KartCheckpointData kart in kartCheckpointData) {
            PlayerInput temp = kart.GetComponent<PlayerInput>();
            if (temp != null) {
                return kart;
            }
        }

        return null;
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
    public void StopTimer() => runTimer = false;
    
    string TimeToString(float currentTime) {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return time.ToString(@"mm\:ss\:ff");
    }
}
