using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostpad : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float impulseBoostAmount;
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag != "Kart") return;

        KartController kc = other.transform.GetComponent<KartMotor>().kartController;
        kc.AddBoostTime(time);
        kc.GiveImpulseBoost(impulseBoostAmount);
    }
}
