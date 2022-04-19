using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWaypoint : MonoBehaviour
{
    public AiWaypoint nextWaypoint;

    public bool startDrifting = false;
    public bool stopDrifting = false;
    public float distanceToReachWaypoint = 20f;

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<KartMotor>(out KartMotor km)) {
            AiInput ai = km.kartController.transform.GetComponent<AiInput>();
            if (ai == null) return;
            ai.SetTargetPosition(nextWaypoint.transform.position);
        }
    }
}
