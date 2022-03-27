using System;
using UnityEngine;

public class KartController : MonoBehaviour
{

    public delegate void KartAction();
    public static event KartAction BoostUsed;

    [SerializeField] Kart kart;

    float horizontal, vertical, moveSpeed, currentBoostTime = 0, direction = 0;
    public bool grounded, drifting, boost, hopped;
    public float currentSpeed, driftValue;

    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundLayer;

    float DriftDirection(float direction) => Mathf.Abs(0.75f - (direction * -horizontal));
    float Steer(float direction, float ammount) => (direction * kart.turnSpeed * Time.deltaTime) * ammount;

    void Start()
    {
        rb.transform.parent = null;
    }

    void Update()
    {
        transform.position = rb.transform.position;
        GroundCheck();
        Steering();

        if (grounded) hopped = false;
    }

    void Steering()
    {   
        if (!drifting) direction = horizontal;
        float turnDirection = drifting ? direction : horizontal;
        float ammount = drifting ? DriftDirection(direction) : Mathf.Abs(horizontal);
        transform.Rotate(0f, Steer(turnDirection, ammount), 0f);
    }
    void GroundCheck()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.6f, groundLayer);
        Debug.DrawRay(transform.position, -transform.up * 0.6f, Color.red);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7f * Time.deltaTime);

        rb.drag = grounded ? kart.groundDrag : kart.airDrag;
    }
    void FixedUpdate()
    {
        currentSpeed = transform.InverseTransformDirection(rb.velocity).z;
        DriftMovement();
        Movement();
        ApplyGravity();
        Drifting();
    }

    void Movement() {
        if (grounded && !drifting) {
            float speed = vertical > 0 ? kart.forwardSpeed : kart.reverseSpeed;
            rb.AddForce(transform.forward * speed * vertical, ForceMode.Acceleration);
        }
    }

    void DriftMovement() {
        if (drifting && grounded) {
            float speed = kart.forwardSpeed - kart.outwardsDriftForce;
            rb.AddForce(transform.right * -direction * kart.outwardsDriftForce * vertical, ForceMode.Acceleration);
            rb.AddForce(transform.forward * speed * vertical, ForceMode.Acceleration);
        }
    }

    void ApplyGravity() {
        if(!grounded)
        rb.AddForce(transform.up * -kart.gravity, ForceMode.Acceleration);
    }

    void Drifting() {
        if (drifting) {
            driftValue += kart.driftChargeSpeed * Time.deltaTime;
        } else if (!drifting && driftValue >= kart.maxDriftCharge) {
            driftValue = 0;
             currentBoostTime = kart.boostTime;
            if (BoostUsed != null)
                BoostUsed();
        } else {
            driftValue = 0;
        }

        if (currentBoostTime > 0) {
            currentBoostTime -= 1f * Time.deltaTime;
            rb.AddForce(transform.forward * kart.driftBoost, ForceMode.Acceleration);
        }
    }

    public void Hop() {
        if (!grounded) return;
        hopped = true;
        rb.AddForce(transform.up * kart.jumpForce, ForceMode.Impulse);
    }

    public void SetInputs(float horizontal, float vertical) {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }
}
