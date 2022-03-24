using System;
using UnityEngine;

public class KartController : MonoBehaviour
{   
    [Serializable]
    public struct KartStats {
        public float forwardSpeed;
        public float reverseSpeed;
        public float turnSpeed;
        public float driftTurnSpeed;
        public float groundDrag;
        public float airDrag;
        public float driftBoost;
        public float driftChargeSpeed;
        public float maxDriftCharge;

        public float boostTime;
    }

    float horizontal, vertical, moveSpeed, driftValue, currentBoostTime = 0;
    bool grounded, drifting;
    public float speed;
    int currentBoostCount;

    public KartStats ks;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] BoostBar bb;



    void Start() {
        rb.transform.parent = null;
        bb.SetMaxValue(ks.maxDriftCharge);
    }

    void Update()
    {
        transform.position = rb.transform.position;
        
        if (!drifting)
            horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0)
            drifting = Input.GetButton("Drift");

        moveSpeed = vertical > 0 ? ks.forwardSpeed : ks.reverseSpeed;
        Drifting();
        GroundCheck();
        if (grounded) {
            Steering();
        }

    }

    float DriftDirection(float direction) {
        return Mathf.Abs(1 - (direction * Input.GetAxisRaw("Horizontal")));
    }

    void Steering() {
        float direction = horizontal == 1 ? 1 : -1;
        float turnSpeed = drifting ? ks.driftTurnSpeed : ks.turnSpeed;
        float rotation = Steer(horizontal, Mathf.Abs(horizontal), turnSpeed);
        if (drifting) {
            if (direction == 1)
                rotation = Steer(direction, DriftDirection(-1), turnSpeed);
            else if (direction == -1)
                rotation = Steer(direction, DriftDirection(1), turnSpeed);
        }
        transform.Rotate(0f, rotation, 0f, Space.World);
    }

    float Steer(float direction, float ammount, float turnSpeed) {
        return (direction * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical")) * ammount;
    }

    void GroundCheck() {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        Debug.DrawRay(transform.position, -transform.up * 1f, Color.red);
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        rb.drag = grounded ? ks.groundDrag : ks.airDrag;
    }

    void Drifting() {

        if (drifting) {
            driftValue += ks.driftChargeSpeed * Time.deltaTime;
        } else if (!drifting && driftValue >= ks.maxDriftCharge) {
            driftValue = 0;
            currentBoostTime = ks.boostTime;
        } else {
            driftValue = 0;
        }

        if (currentBoostTime > 0) {
            moveSpeed = ks.driftBoost;
            currentBoostTime -= 1f * Time.deltaTime;
        }

        bb.SetValue(driftValue);
    }

    void FixedUpdate() {
        speed = transform.InverseTransformDirection(rb.velocity).z;
        // rb.AddForce(transform.forward * moveSpeed * vertical, ForceMode.Acceleration); rb.velocity = transform.forward * moveSpeed * vertical;
        if (grounded)
            rb.AddForce(transform.forward * moveSpeed * vertical, ForceMode.Acceleration);
        else
            rb.AddForce(transform.up * -20f, ForceMode.Acceleration);
    }
}
