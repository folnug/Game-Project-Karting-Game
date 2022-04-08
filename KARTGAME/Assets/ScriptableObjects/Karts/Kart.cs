using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kart", menuName = "KART DEMO/Kart", order = 0)]
public class Kart : ScriptableObject {
    [Header("Movement Settings")]
    public float forwardSpeed;

    public float Acceleration;
    public float Decelerate;

    public float brakeForce;
    public float reverseSpeed;
    public float turnSpeed;
    public float jumpForce;

    [Header("Driftin Settings")]
    public float driftTurnSpeed;
    public float driftTurnModifier;
    public float driftChargeSpeed;
    public float outwardsDriftForce;

    [Header("Boost Settings")]
    public float boostTime;
    public float boostAmount;
    public float impulseBoostAmount;

    [Header("Air Settings")]
    public float airDrag;
    public float groundDrag;

    [Header("Visual Turning Settings")]
    public float rollAmount;
    public float rollDriftAmount;
    public float yawAmount;
    public float yawDriftAmount;
    public float visualTurningSpeed;
}
