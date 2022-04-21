using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class CheckpointHandler : MonoBehaviour
{
    List<Checkpoint> checkpoints = new List<Checkpoint>();
    float timer = 0f;
    List<KartCheckpointData> kartCheckpointData = new List<KartCheckpointData>();

    void Awake() {

        KartCheckpointData[] tempKartCheckpointData = FindObjectsOfType<KartCheckpointData>();

        Checkpoint[] tempCheckpoints = transform.GetComponentsInChildren<Checkpoint>();
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
        timer += Time.deltaTime;
        KartPosition();
        for (int i = 0; i < kartCheckpointData.Count; i++) {
            kartCheckpointData[i].position = i + 1;
        }
    }
    public void KartEnteredCheckpoint(KartCheckpointData data, Checkpoint checkpoint) {
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
        kartCheckpointData.Sort((kart1, kart2) => {
            if (kart1.CheckpointsCollected != kart2.CheckpointsCollected)
                return kart1.CheckpointsCollected.CompareTo(kart2.CheckpointsCollected);
            
            return kart2.DistanceToNext().CompareTo(kart1.DistanceToNext());
        });
        kartCheckpointData.Reverse();

    }

    string TimeToString(float currentTime) {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return time.ToString(@"mm\:ss\:ff");
    }
}
