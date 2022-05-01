using System;
using UnityEngine;

public class KartController : MonoBehaviour
{
    public Kart kart;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform visual;

    [SerializeField] float rubberBandSpeed = 20f;

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
    float airTime = 0f;

    float speedEffector = 1f;
    bool hoppedBeforAirborne = false;

    // Automatic
    float automaticDriftTimer = 0f;
    float activateAutomaticDriftTime = 0.5f;
    bool automaticDrift = false;
    float automaticDriftDirection = 0f;

    float rubberBandSpeedEffector = 0f;

    #endregion

    public enum KartStates {
        Stunned,
        Drive,
        Spun,
    }

    public KartStates currentState { get; private set; }

    public enum KartDriftModes {
        Automatic = 0,
        Normal = 1,
    }

    public KartDriftModes driftMode;

    float DriftDirection(float direction) => Mathf.Abs(kart.driftTurnModifier - (direction * -horizontal));
    float Steer(float turnSpeed, float direction, float amount) => (direction * turnSpeed * Time.deltaTime) * amount;

    void Awake()
    {
        rb.transform.parent = null;
        minDriftAmmount = 80f;

        currentState = KartStates.Stunned;
        driftMode = KartDriftModes.Normal;
        automaticDriftTimer = activateAutomaticDriftTime;
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

        if (hit.transform != null)
            speedEffector = hit.transform.tag == "Road" ? 1f: 0.5f;

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
                if (driftMode == KartDriftModes.Normal) {
                    DriftChecks();
                    Drifting();
                } else if (driftMode == KartDriftModes.Automatic) {
                    AutomaticDrift();
                }
                AirTime();
                break;
            case KartStates.Spun:
                break;
        }
    }

    void AutomaticDrift() {
        if (horizontal > 0.5f) {
            automaticDrift = true;
            automaticDriftDirection = 1f;
        } else if (horizontal < -0.5f) {
            automaticDrift = true;
            automaticDriftDirection = -1f;
        }
        else automaticDrift = false;
        
        if(automaticDrift && direction == automaticDriftDirection)
            automaticDriftTimer = Mathf.Clamp(automaticDriftTimer - Time.deltaTime, 0, activateAutomaticDriftTime);
        else 
            automaticDriftTimer = activateAutomaticDriftTime;
        
        if (automaticDriftTimer <= 0)
            drifting = true;
        else {
            drifting = false;
            automaticDrift = false;
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

        if (grounded) {
            rb.AddForce(transform.forward * currentSpeed * speedEffector, ForceMode.Force);
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

        currentSpeed = Mathf.Clamp(currentSpeed, 0, kart.forwardSpeed + (rubberBandSpeed * rubberBandSpeedEffector));
    }

    void BackwardsMovement() {
        if (vertical < 0) {
            currentSpeed -= kart.Acceleration * Time.deltaTime;
        } else {
            currentSpeed += kart.Decelerate * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -kart.reverseSpeed, 0f);

    }

    void DriftChecks() {
        if (hopped) {
            rb.AddForce(transform.up * kart.jumpForce, ForceMode.Impulse);
            hopped = false;
            if ((horizontal != 0) && currentSpeed > kart.forwardSpeed * 0.25f) {
                drifting = true;
            }
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

        SoundManager.PlaySound(SoundManager.Sound.KartHop, transform.position, 0.01f);
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
        SoundManager.PlaySound(SoundManager.Sound.KartBoost, transform.position, 0.05f);
    }

    public void SetInputs(float horizontal, float vertical) {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }

    public void AddBoostTime(float amount) => currentBoostTime += amount;

    public void SetState(KartStates state) => currentState = state;

    public float GetVertical() => vertical;
    public float GetHorizontal() => horizontal;

    public void SetSpeedEffector(float amount) => rubberBandSpeedEffector = amount;

    public float GetDirection() {
        if (!drifting)  return 0;
        return direction;
    }

}