using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAiWayPoints : MonoBehaviour
{
    void OnDrawGizmos() {
        AiWaypoint[] aiWaypoints = GetComponentsInChildren<AiWaypoint>();
        AiWaypoint lastWaypoint = aiWaypoints[0];
        foreach(AiWaypoint waypoints in aiWaypoints) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(lastWaypoint.transform.position, lastWaypoint.nextWaypoint.transform.position);
            lastWaypoint = lastWaypoint.nextWaypoint;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(lastWaypoint.transform.position, 1f);
        }
    }
}
