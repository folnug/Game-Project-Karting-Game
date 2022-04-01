using System;
using UnityEngine;

public class KartController : MonoBehaviour
{
    [SerializeField] Kart kart;

    float horizontal, vertical, moveSpeed, direction = 0;

    public bool grounded { get; private set; }

    public bool braking { get; private set; }
    public bool drifting { get; private set; }
    public bool hopped { get; set; }

    public float speed { get; private set; }
    public float driftValue { get; private set; }
    public float currentBoostTime { get; private set; }

    public float minDriftAmmount { get; private set; }

    bool giveImpulseBoost = false;
    float impulseBoostAmount = 0f;

    float currentSpeed = 0;

    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform visual;

    float DriftDirection(float direction) => Mathf.Abs(kart.driftTurnModifier - (direction * -horizontal));
    float Steer(float turnSpeed, float direction, float amount) => (direction * turnSpeed * Time.deltaTime) * amount;

    void Start()
    {
        rb.transform.parent = null;
        minDriftAmmount = 80f;
    }

    void Update()
    {
        transform.position = rb.transform.position;
        GroundCheck();
        Steering();
    }

    void Steering()
    {   
        if (!drifting) direction = horizontal > 0 ? 1f : -1f;
        float turnDirection = drifting ? direction : horizontal;
        float turnSpeed = drifting && grounded ? kart.driftTurnSpeed : kart.turnSpeed;
        float amount = drifting ? DriftDirection(direction) : Mathf.Abs(horizontal);
        transform.Rotate(0f, Steer(turnSpeed, turnDirection, amount), 0f);


        // Visual
        float visualDirection = drifting ? direction : horizontal;
        float localVisualRotAmmoung = drifting ? 10f : 20f;
        int isMotorcycle = 0;
        visual.localRotation = Quaternion.Lerp(visual.localRotation, Quaternion.Euler(0, localVisualRotAmmoung * visualDirection, 30f * -visualDirection * isMotorcycle), 2f * Time.deltaTime);

    }
    void GroundCheck()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.8f, groundLayer);
        Debug.DrawRay(transform.position, -transform.up * 0.8f, Color.red);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7f * Time.deltaTime);

        rb.drag = grounded ? kart.groundDrag : kart.airDrag;
    }
    void FixedUpdate()
    {
        speed = transform.InverseTransformDirection(rb.velocity).z;
        Movement();
        //ApplyGravity();
        Drifting();

        if (giveImpulseBoost) {
            rb.AddForce(transform.forward * impulseBoostAmount, ForceMode.Impulse);
            giveImpulseBoost = false;
        }
    }

    void Movement() {
        if ((vertical != 0 || horizontal != 0) && OnSlope()) {
            rb.AddForce(-transform.up * 70f, ForceMode.Acceleration);
        }
        
        braking = vertical < 0 && currentSpeed > 0;
        if (currentSpeed <= 0 && vertical < 0) {
            BackwardsMovement();
        } else if (grounded) {
            ForwardMovement();
        }

        if (drifting && grounded) {
            rb.AddForce(transform.right * -direction * kart.outwardsDriftForce, ForceMode.Acceleration);
        }

        if (hopped) {
            rb.AddForce(transform.up * kart.jumpForce, ForceMode.Impulse);
            if ((horizontal != 0) && currentSpeed > kart.forwardSpeed * 0.25f) {
                drifting = true;
            }
            hopped = false;
        }
    }

    void ForwardMovement() {
        if (vertical > 0) {
            currentSpeed += kart.Acceleration * Time.deltaTime;
        } else if (vertical < 0) {
            currentSpeed -= kart.brakeForce * Time.deltaTime;
        } else if (vertical == 0) {
            currentSpeed -= kart.Decelerate * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, kart.forwardSpeed);
        float moveSpeed = drifting ? currentSpeed - kart.outwardsDriftForce : currentSpeed;
        if (grounded) {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Force);
        }
    }

    void BackwardsMovement() {
        if (vertical < 0) {
            currentSpeed -= kart.Acceleration * Time.deltaTime;
        } else {
            currentSpeed += kart.Decelerate * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -kart.reverseSpeed, 0f);
        if (grounded) {
            rb.AddForce(transform.forward * currentSpeed, ForceMode.Force);
        }
    }

    void ApplyGravity() {
        if(!grounded)
        rb.AddForce(transform.up * -kart.gravity, ForceMode.Acceleration);
    }

    void Drifting() {
        if (drifting && grounded) {
            driftValue += kart.driftChargeSpeed * Time.deltaTime;
        } else if (!drifting && driftValue >= minDriftAmmount) {
            driftValue = 0;
            AddBoostTime(kart.boostTime);
            GiveImpulseBoost(kart.impulseBoostAmount);
        } else if (!drifting) {
            driftValue = 0;
        }
        if (currentBoostTime > 0) {
            currentBoostTime -= 1f * Time.deltaTime;
            rb.AddForce(transform.forward * kart.boostAmount, ForceMode.Acceleration);
        }
    }

    bool OnSlope() {
        if (!hopped) return false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.8f))
            if (hit.normal != Vector3.up) return true;
        return false;

    }

    public void Hop() {
        if (!grounded) return;
        hopped = true;
    }

    public void StopDrifting() {
        drifting = false;
    }


    public void GiveImpulseBoost(float amount) {
        giveImpulseBoost = true;
        impulseBoostAmount = amount;
    }

    public void SetInputs(float horizontal, float vertical) {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }

    public void AddBoostTime(float amount) => currentBoostTime += amount;

}





