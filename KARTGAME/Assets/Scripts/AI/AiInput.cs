using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AiInput : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 targetWaypoint;
    KartController kartController;

    AiWaypoint currentWaypoint;
    AiWaypoint lastCheckpoint;
    AiWaypoint[] aiWaypoints;

    float vertical = 0f;
    float horizontal = 0f;

    bool hopped = false;

    void Start()
    {
        kartController = GetComponent<KartController>();
        aiWaypoints = FindObjectsOfType<AiWaypoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (currentWaypoint == null) {
            currentWaypoint = FindClosestWaypoint();
            lastCheckpoint = currentWaypoint;
        }
        
        if (currentWaypoint == null) return;

        //SetTargetPosition(targetPositionTransform.position);

        targetPosition = currentWaypoint.transform.position;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        float distaneToPoint = (targetPosition - transform.position).magnitude;

        
        if (distaneToPoint > 40f) {
            Vector3 nearestPointOnWaypoint = FindNearestPointOnLine(lastCheckpoint.transform.position, targetWaypoint, transform.position);
            float segments = distaneToPoint / 40f;

            targetPosition = (targetPosition + nearestPointOnWaypoint * segments) / (segments + 1);
            Debug.DrawLine(transform.position, targetPosition, Color.blue);
        } 
        
        if (distanceToTarget <= currentWaypoint.distanceToReachWaypoint) {
            lastCheckpoint = currentWaypoint;
            currentWaypoint = currentWaypoint.nextWaypoint;
            targetPosition = currentWaypoint.transform.position;
            targetWaypoint = randomPosition(currentWaypoint.transform.position);
        }

        Vector3 directionToMovePosition = (targetPosition - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToMovePosition);

        float angleToDirection = Vector3.SignedAngle(transform.forward, directionToMovePosition, Vector3.up);
        if (currentWaypoint.stopDrifting) {
            kartController.StopDrifting();
            hopped = false;
        }
        else if (currentWaypoint.startDrifting && !hopped) {
            kartController.Hop();
            hopped = true;
        } 

        horizontal = angleToDirection / 10.0f;

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

    Vector3 FindNearestPointOnLine(Vector3 starPos, Vector3 endPos, Vector3 point) {
        Vector3 headingVector = (endPos - starPos);

        float maxDistance = headingVector.magnitude;
        headingVector.Normalize();

        Vector3 vectorStartToPoint = point - starPos;
        float dot = Vector3.Dot(vectorStartToPoint, headingVector);

        dot = Mathf.Clamp(dot, 0f, maxDistance);

        return starPos + headingVector * dot;
    }

    Vector3 randomPosition(Vector3 position) {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return position + randomDirection * Random.Range(1f, 25f);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(targetPosition, 2f);
    }
}
