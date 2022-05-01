using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class KartCheckpointData : MonoBehaviour
{
    public List<float> lastTimes = new List<float>();
    public List<float> times = new List<float>();
    public int laps = 0;
    public int CheckpointsCollected = 0;
    public int currentCheckpointIndex = 0;
    public int nextCheckpointIndex = 0;
    public int position = 0;
    public int maxLaps;
    public Checkpoint nextCheckpoint;

    string kartName;

    void Awake() {
        kartName = GetComponent<KartController>().kart.Name;
    }

    public string GetKartName() => kartName;
    public float DistanceToNext() =>
        Vector3.Distance(transform.position, nextCheckpoint.transform.position);
}
