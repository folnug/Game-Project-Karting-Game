using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    List<int> nextCheckpoints = new List<int>();
    List<int> laps = new List<int>();
    List<Checkpoint> checkpoints = new List<Checkpoint>();
    [SerializeField] List<Transform> karts;


    void Awake() {
        Checkpoint[] tempCheckpoints = transform.GetComponentsInChildren<Checkpoint>();
        foreach(Checkpoint checkpoint in tempCheckpoints) {
            checkpoints.Add(checkpoint);
            checkpoint.SetCheckpointHandler(this);
        }

        foreach(Transform kart in karts) {
            nextCheckpoints.Add(0);
            laps.Add(0);
        }
    }
    public void KartEnteredCheckpoint(Transform kart, Checkpoint checkpoint) {
        int nextCheckpoint = nextCheckpoints[karts.IndexOf(kart)];
        if (checkpoints.IndexOf(checkpoint) == nextCheckpoint) {
            //nextCheckpoints[karts.IndexOf(kart)] = (nextCheckpoint + 1) % checkpoints.Count;
            nextCheckpoints[karts.IndexOf(kart)] = nextCheckpoint + 1;
            if (nextCheckpoints[karts.IndexOf(kart)] == checkpoints.Count) {
                laps[karts.IndexOf(kart)] += 1;
                nextCheckpoints[karts.IndexOf(kart)] = nextCheckpoints[karts.IndexOf(kart)] % checkpoints.Count;
            }
        } else {
            Debug.Log("wrong checkpoint!");
        }
    }
}
