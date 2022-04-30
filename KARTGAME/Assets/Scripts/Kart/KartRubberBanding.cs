using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartRubberBanding : MonoBehaviour
{
    KartController kart;
    KartCheckpointData kartData;

    CheckpointHandler checkpointHandler;

    float minDistance = 20f;
    float maxDistance = 100f;
    void Awake() {
        kart = GetComponent<KartController>();
        kartData = GetComponent<KartCheckpointData>();
        checkpointHandler = FindObjectOfType<CheckpointHandler>();
    }

    void FixedUpdate() {
        /*
        if (kartData.position == 1) return;

        Transform firstPositionKart = checkpointHandler.FirstPositionKart();
        float distanceBetween = Vector3.Distance(transform.position, firstPositionKart.position);
        float speedEffector = 0;
        if (distanceBetween > minDistance) {
            speedEffector = Mathf.Clamp(distanceBetween / maxDistance, 0f, 1f);
            Debug.Log(transform.name + " " + speedEffector);
        }

        kart.SetSpeedEffector(speedEffector);
        */
    }
}
