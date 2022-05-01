using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartRubberBanding : MonoBehaviour
{
    KartController kart;
    KartCheckpointData kartData;

    CheckpointHandler checkpointHandler;

    Transform kartInFirst;

    float minDistance = 20f;
    float maxDistance = 100f;

    void Awake() {
        kart = GetComponent<KartController>();
        kartData = GetComponent<KartCheckpointData>();
        checkpointHandler = FindObjectOfType<CheckpointHandler>();
    }

    void OnEnable() {
        CheckpointHandler.KartInFirstPos += SetKartInFirst;
    }

    void OnDisable() {
        CheckpointHandler.KartInFirstPos -= SetKartInFirst;
    }

    void SetKartInFirst(KartCheckpointData kart) {
        kartInFirst = kart.transform;
    }

    void FixedUpdate() {
        if (kartData.position == 1 || kartInFirst == null) return;

        float distanceBetween = Vector3.Distance(transform.position, kartInFirst.position);
        float speedEffector = 0;
        if (distanceBetween > minDistance) {
            speedEffector = Mathf.Clamp(distanceBetween / maxDistance, 0f, 1f);
        }
        kart.SetSpeedEffector(speedEffector);
    }
}
