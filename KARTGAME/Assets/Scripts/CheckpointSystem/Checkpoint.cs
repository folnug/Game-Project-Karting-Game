using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointHandler checkpointHandler;

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<KartMotor>(out KartMotor kartMotor)) {
            KartCheckpointData data = kartMotor.parent.GetComponent<KartCheckpointData>();
            checkpointHandler.KartEnteredCheckpoint(data, this);
        }
    }

    public void SetCheckpointHandler(CheckpointHandler ch) {
        checkpointHandler = ch;
    }
}
