using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Ghost : ScriptableObject
{
    public float totalTime;
    public bool record;
    public bool replay;
    public float recordFrequency = 240f;
    public GameObject character;
    public List<float> timestamp;
    public List<Vector3> position;
    public List<Quaternion> rotation;

    public void Reset() {
        timestamp.Clear();
        position.Clear();
        rotation.Clear();
    }
}
