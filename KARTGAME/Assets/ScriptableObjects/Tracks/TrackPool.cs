using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrackPool", menuName = "KART DEMO/Track/TrackPool", order = 2)]
public class TrackPool: ScriptableObject
{
    public Track[] tracks;
    public int currentTrackIndex;
}
