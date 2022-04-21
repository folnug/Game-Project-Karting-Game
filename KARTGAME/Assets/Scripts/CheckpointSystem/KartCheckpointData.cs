using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KartCheckpointData : MonoBehaviour
{
    public List<float> lastTimes = new List<float>();
    public List<float> times = new List<float>();
    public int laps = 0;
    public int CheckpointsCollected = 0;
    public int nextCheckpoint = 0;
    public int position = 0;
}
