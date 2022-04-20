using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AiInput : MonoBehaviour
{
    Vector3 targetPosition;
    KartController kartController;

    AiWaypoint currentWaypoint;
    AiWaypoint lastCheckpoint;
    AiWaypoint[] aiWaypoints;

    bool hopped = false;

    void Start()
    {
        kartController = GetComponent<KartController>();
        aiWaypoints = FindObjectsOfType<AiWaypoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (currentWaypoint == null)
            currentWaypoint = FindClosestWaypoint();
        
        if (currentWaypoint == null) return;

        //SetTargetPosition(targetPositionTransform.position);

        float vertical = 0f;
        float horizontal = 0f;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= currentWaypoint.distanceToReachWaypoint) {
            lastCheckpoint = currentWaypoint;
            currentWaypoint = currentWaypoint.nextWaypoint;
            SetTargetPosition(randomPosition(currentWaypoint.transform.position));
        }

        Vector3 directionToMovePosition = (targetPosition - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToMovePosition);

        float angleToDirection = Vector3.SignedAngle(transform.forward, directionToMovePosition, Vector3.up);

        if (currentWaypoint.startDrifting && !hopped) {
            kartController.Hop();
            hopped = true;
        } else if (currentWaypoint.stopDrifting) {
            kartController.StopDrifting();
            hopped = false;
        }

        horizontal = angleToDirection / 25.0f;

        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = dot > 0 ? 1f : -1f;

        vertical = vertical * (1.05f - Mathf.Abs(horizontal) / 1.0f);


        kartController.SetInputs(horizontal, vertical);
    }


    public void SetTargetPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    AiWaypoint FindClosestWaypoint() {
        return aiWaypoints
        .OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    }

    Vector3 randomPosition(Vector3 position) {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return position + randomDirection * Random.Range(1f, 15f);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(targetPosition, 2f);
    }
}
