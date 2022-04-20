using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class KartController : MonoBehaviour
{
    [SerializeField] Kart kart;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform visual;
    [SerializeField] float driftBufferTimeDuration = 0.5f;
    #region public variables
    public bool grounded { get; private set; }
    public bool giveImpulseBoost { get; private set; }
    public bool braking { get; private set; }
    public bool drifting { get; private set; }
    public bool hopped { get; set; }
    public float speed { get; private set; }
    public float driftValue { get; private set; }
    public float currentBoostTime { get; private set; }
    public float minDriftAmmount { get; private set; }
    #endregion

    #region private variables
    float horizontal, vertical, moveSpeed, direction = 0;
    float impulseBoostAmount = 0f;
    float currentSpeed = 0;
    float driftBufferTime = 0;
    float airTime = 0f;
    bool hoppedBeforAirborne = false;
    #endregion

    public enum KartStates {
        Stunned,
        Drive,
        Spun,
    }

    public KartStates currentState { get; private set; }

    float DriftDirection(float direction) => Mathf.Abs(kart.driftTurnModifier - (direction * -horizontal));
    float Steer(float turnSpeed, float direction, float amount) => (direction * turnSpeed * Time.deltaTime) * amount;

    void Start()
    {
        rb.transform.parent = null;
        minDriftAmmount = 80f;

        currentState = KartStates.Stunned;
    }

    void Update()
    {
        transform.position = rb.transform.position;
        GroundCheck();
    }

    void Steering()
    {   
        if (!drifting) direction = horizontal > 0 ? 1f : -1f;
        float turnDirection = drifting ? direction : horizontal;
        float turnSpeed = drifting && grounded ? kart.driftTurnSpeed : kart.turnSpeed;
        float amount = drifting ? DriftDirection(direction) : Mathf.Abs(horizontal);
        transform.Rotate(0f, Steer(turnSpeed, turnDirection, amount), 0f);

        float visualDirection = drifting ? direction : horizontal;
        float localVisualYawAmount = drifting ? kart.yawDriftAmount : kart.yawAmount;
        float localVisualRollAmount = drifting ? kart.rollDriftAmount : kart.rollAmount;
        visual.localRotation = Quaternion.Lerp(visual.localRotation, Quaternion.Euler(0, localVisualYawAmount * visualDirection, localVisualRollAmount * -visualDirection), kart.visualTurningSpeed * Time.deltaTime);

    }
    void GroundCheck()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.8f, groundLayer);
        Debug.DrawRay(transform.position, -transform.up * 0.8f, Color.red);
        Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7f * Time.deltaTime);
        
        rot.x = Mathf.Clamp(rot.x, -45f, 45f);
        rot.z = Mathf.Clamp(rot.z, -45f, 45f);
        transform.rotation = rot;
        
        rb.drag = grounded ? kart.groundDrag : kart.airDrag;
    }
    void FixedUpdate()
    {
        speed = transform.InverseTransformDirection(rb.velocity).z;
        switch(currentState) {
            case KartStates.Stunned:
                break;
            case KartStates.Drive:
                Steering();
                Movement();
                DriftChecks();
                Drifting();
                AirTime();
                break;
            case KartStates.Spun:
                break;
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

        if (drifting && grounded && vertical > 0) {
            rb.AddForce(transform.right * -direction * kart.outwardsDriftForce, ForceMode.Acceleration);
        }
        #region Boosts
        if (giveImpulseBoost) {
            //Debug.Log("Give impulse boost!, "+ impulseBoostAmount);
            rb.AddForce(transform.forward * impulseBoostAmount, ForceMode.Impulse);
            giveImpulseBoost = false;
        }

        if (currentBoostTime > 0) {
            currentBoostTime -= 1f * Time.deltaTime;
            rb.AddForce(transform.forward * kart.boostAmount, ForceMode.Acceleration);
        }
        #endregion
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
        if (grounded) {
            rb.AddForce(transform.forward * currentSpeed, ForceMode.Force);
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

    void DriftChecks() {
        if (hopped) {
            rb.AddForce(transform.up * kart.jumpForce, ForceMode.Impulse);
            driftBufferTime = driftBufferTimeDuration;
            hopped = false;
        }
        if (driftBufferTime > 0) driftBufferTime -= Time.deltaTime;
        if (driftBufferTime <= 0) driftBufferTime = 0;
        if ((horizontal != 0) && currentSpeed > kart.forwardSpeed * 0.25f && driftBufferTime > 0) {
            drifting = true;
            driftBufferTime = 0;
        }

        if (drifting && vertical < 0) {
            drifting = false;
            currentBoostTime = 0;
            driftValue = 0;
        }
    }

    void Drifting() {
        if (drifting && grounded) {
            driftValue += kart.driftChargeSpeed * Time.deltaTime;
        } else if (!drifting && driftValue >= minDriftAmmount && vertical > 0) {
            driftValue = 0;
            AddBoostTime(kart.boostTime);
            GiveImpulseBoost(kart.impulseBoostAmount);
        } else if (!drifting) {
            driftValue = 0;
        }
    }

    bool OnSlope() {
        if (!hopped) return false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.8f))
            if (hit.normal != Vector3.up) return true;
        return false;

    }

    void AirTime() {
        if (Landed() && airTime >= 0.6 && hoppedBeforAirborne)  {
            GiveImpulseBoost(kart.boostAmount);
            AddBoostTime(kart.boostTime);
        }

        if (Landed()) hoppedBeforAirborne = false;
        
        if (grounded) {
            airTime = 0f;
        } else {
            airTime += Time.deltaTime;
        }
    }
    public void Hop() {
        if (!grounded) return;
        hoppedBeforAirborne = true;
        hopped = true;

        //SoundController.PlaySound(SoundController.Sound.KartHop, transform.position, 0.2f);
    }

    public void StopDrifting() {
        drifting = false;
    }
    public bool Landed() {
        return airTime > 0 && grounded;
    }

    public void GiveImpulseBoost(float amount) {
        giveImpulseBoost = true;
        impulseBoostAmount = amount;
        //SoundController.PlaySound(SoundController.Sound.KartBoost, transform.position, 0.2f);
    }

    public void SetInputs(float horizontal, float vertical) {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }

    public void AddBoostTime(float amount) => currentBoostTime += amount;

    public void SetState(KartStates state) => currentState = state;

    public float GetVertical() => vertical;

}