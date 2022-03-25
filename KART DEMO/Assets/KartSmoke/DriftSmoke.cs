using UnityEngine;
using System.Collections;
using System;

public class DriftSmoke : MonoBehaviour
{
    [SerializeField] KartController kc;
    [SerializeField] ParticleSystem leftSmoke, rightSmoke;

    void Start()
    {

    }


    void Update()
    {
        CheckIfDrifting();
    }

    void CheckIfDrifting()
    {
        if (kc.drifting && kc.grounded)
        {
            leftSmoke.Play();
            rightSmoke.Play();
        }
    }
}