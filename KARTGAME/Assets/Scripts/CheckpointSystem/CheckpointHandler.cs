using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class KartTimes {
    public List<float> lastTimes = new List<float>();
    public List<float> times = new List<float>();
}

public class CheckpointHandler : MonoBehaviour
{
    List<int> nextCheckpoints = new List<int>();
    List<int> laps = new List<int>();
    List<Checkpoint> checkpoints = new List<Checkpoint>();

    List<KartTimes> times = new List<KartTimes>();

    public List<int> CheckpointsCollected = new List<int>();

    public List<Transform> karts = new List<Transform>();

    List<int> kartPosition = new List<int>();

    float timer = 0f;


    void Awake() {

        KartController[] kartsTemp = FindObjectsOfType<KartController>();

        foreach(KartController kartTemp in kartsTemp) {
            karts.Add(kartTemp.transform);
        }

        Checkpoint[] tempCheckpoints = transform.GetComponentsInChildren<Checkpoint>();
        foreach(Checkpoint checkpoint in tempCheckpoints) {
            checkpoints.Add(checkpoint);
            checkpoint.SetCheckpointHandler(this);
        }

        foreach(Transform kart in karts) {
            nextCheckpoints.Add(0);
            laps.Add(0);
            times.Add(new KartTimes());
            times[karts.IndexOf(kart)].lastTimes.Add(0);
            CheckpointsCollected.Add(0);
        }

        for (int i = 0; i < karts.Count; i++) {
            kartPosition.Add(i + 1);
        }
    }

    void FixedUpdate() {
        timer += Time.deltaTime;
        kartPositions();
    }
    public void KartEnteredCheckpoint(Transform kart, Checkpoint checkpoint) {
        int nextCheckpoint = nextCheckpoints[karts.IndexOf(kart)];
        if (checkpoints.IndexOf(checkpoint) == nextCheckpoint) {
            nextCheckpoints[karts.IndexOf(kart)] = nextCheckpoint + 1;
            CheckpointsCollected[karts.IndexOf(kart)] += 1;
            if (nextCheckpoints[karts.IndexOf(kart)] == checkpoints.Count) {
                times[karts.IndexOf(kart)].times.Add(timer - times[karts.IndexOf(kart)].lastTimes[laps[karts.IndexOf(kart)]]);
                times[karts.IndexOf(kart)].lastTimes.Add(timer);

                laps[karts.IndexOf(kart)] += 1;
                nextCheckpoints[karts.IndexOf(kart)] = nextCheckpoints[karts.IndexOf(kart)] % checkpoints.Count;
            }
        } else {
            return;
        }
    }
    
    void kartPositions() {

    }

    string TimeToString(float currentTime) {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return time.ToString(@"mm\:ss\:ff");
    }
}
