using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CheckpointHandler : MonoBehaviour
{
    List<Checkpoint> checkpoints = new List<Checkpoint>();
    float timer = 0f;
    KartCheckpointData[] kartCheckpointData;

    void Awake() {

        kartCheckpointData = FindObjectsOfType<KartCheckpointData>();

        Checkpoint[] tempCheckpoints = transform.GetComponentsInChildren<Checkpoint>();
        foreach(Checkpoint checkpoint in tempCheckpoints) {
            checkpoints.Add(checkpoint);
            checkpoint.SetCheckpointHandler(this);
        }

        foreach(KartCheckpointData data in kartCheckpointData) {
            data.lastTimes.Add(0);
        }
    }

    void FixedUpdate() {
        timer += Time.deltaTime;
    }
    public void KartEnteredCheckpoint(KartCheckpointData data, Checkpoint checkpoint) {
        int nextCheckpoint = data.nextCheckpoint;
        if (checkpoints.IndexOf(checkpoint) == nextCheckpoint) {
            data.nextCheckpoint = nextCheckpoint + 1;
            data.CheckpointsCollected += 1;
            if (data.nextCheckpoint == checkpoints.Count) {
                data.times.Add(timer - data.lastTimes[data.laps]);
                data.lastTimes.Add(timer);

                data.laps += 1;
                data.nextCheckpoint = data.nextCheckpoint % checkpoints.Count;
            }
        }
    }

    string TimeToString(float currentTime) {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return time.ToString(@"mm\:ss\:ff");
    }
}
