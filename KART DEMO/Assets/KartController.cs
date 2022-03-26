using System;
using UnityEngine;

public class KartController : MonoBehaviour
{
    [Serializable]
    public struct KartStats
    {
        public float forwardSpeed;
        public float reverseSpeed;
        public float turnSpeed;
        public float driftTurnSpeed;
        public float groundDrag;
        public float airDrag;
        public float driftBoost;
        public float driftChainBoost;
        public float driftChargeSpeed;
        public float maxDriftCharge;

        public float boostTime;

        public float outwardDriftForce;

        public int maxBoostCount;
    }

    float horizontal, vertical, moveSpeed, currentBoostTime = 0, direction;
    public bool grounded, drifting, driftFailed = false, boost;
    public float speed, driftValue;

    int currentBoostCount;

    public KartStats ks;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] BoostBar bb;

    [SerializeField] bool chainDrift = true;



    void Start()
    {
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

        boost = Input.GetButton("Jump");

        moveSpeed = vertical > 0 ? ks.forwardSpeed : ks.reverseSpeed;
        GroundCheck();
        if (grounded)
        {
            Steering();
        }

    }

    float DriftDirection(float direction)
    {
        return Mathf.Abs(1 - (direction * Input.GetAxisRaw("Horizontal")));
    }

    void Steering()
    {
        if (!drifting)
            direction = horizontal == 1 ? 1 : -1;
        float turnSpeed = drifting ? ks.driftTurnSpeed : ks.turnSpeed;
        float rotation = Steer(horizontal, Mathf.Abs(horizontal), turnSpeed);
        if (drifting)
        {
            if (direction == 1)
                rotation = Steer(direction, DriftDirection(-1), turnSpeed);
            else if (direction == -1)
                rotation = Steer(direction, DriftDirection(1), turnSpeed);
        }
        transform.Rotate(0f, rotation, 0f, Space.World);
    }

    float Steer(float direction, float ammount, float turnSpeed)
    {
        return (direction * turnSpeed * Time.deltaTime) * ammount;
    }

    void GroundCheck()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        Debug.DrawRay(transform.position, -transform.up * 1f, Color.red);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7f * Time.deltaTime);

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


    void ChainDrifting()
    {
        if (drifting && !driftFailed)
        {
            if (currentBoostCount < ks.maxBoostCount) driftValue += ks.driftChargeSpeed * Time.deltaTime;

            if (driftValue > ks.maxDriftCharge) {
                driftFailed = true;
                driftValue = 0;
                Debug.Log("Drift failed!");
            }

            if (boost && driftValue < ks.maxDriftCharge && driftValue > ks.maxDriftCharge * 0.6)
            {
                driftValue = 0;
                currentBoostCount += 1;

                //currentBoostTime = ks.boostTime;
                 rb.AddForce(transform.forward * ks.driftChainBoost * (ks.maxBoostCount / ks.maxBoostCount) * 1.5f, ForceMode.Impulse);
                Debug.Log("Drift successfull!");
            }
        }
        else if (!drifting)
        {
            driftFailed = false;
            driftValue = 0;
            currentBoostCount = 0;
        }

        if (currentBoostTime > 0) {
            //moveSpeed = ks.driftBoost * (ks.maxBoostCount / ks.maxBoostCount);
            rb.AddForce(transform.forward * ks.driftBoost * (ks.maxBoostCount / ks.maxBoostCount) * Time.deltaTime, ForceMode.Acceleration);
            currentBoostTime -= 1f * Time.deltaTime;
        }

        bb.SetValue(driftValue);
    }

    void FixedUpdate()
    {
        speed = transform.InverseTransformDirection(rb.velocity).z;
        // rb.AddForce(transform.forward * moveSpeed * vertical, ForceMode.Acceleration); rb.velocity = transform.forward * moveSpeed * vertical;
        if (chainDrift) ChainDrifting();
        else Drifting();

        if (drifting)
        {
            rb.AddForce(transform.right * -direction * ks.outwardDriftForce * vertical, ForceMode.Acceleration);
        }

        if (grounded)
            rb.AddForce(transform.forward * moveSpeed * vertical, ForceMode.Acceleration);
        else
            rb.AddForce(transform.up * -20f, ForceMode.Acceleration);
    }
}
