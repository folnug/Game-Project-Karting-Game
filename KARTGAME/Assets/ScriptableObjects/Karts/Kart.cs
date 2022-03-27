using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kart", menuName = "KART DEMO/Kart", order = 0)]
public class Kart : ScriptableObject {
    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;
    public float driftTurnSpeed;
    public float groundDrag;
    public float airDrag;
    public float driftBoost;
    public float maxDriftCharge;
    public float driftChargeSpeed;
    public float boostTime;
    public float outwardsDriftForce;

    public float gravity;
    public float jumpForce;
}
