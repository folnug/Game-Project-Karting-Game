using UnityEngine;
using System.Collections;
using System;

public class DriftSmoke : MonoBehaviour
{
    [SerializeField] KartController kc;
    [SerializeField] ParticleSystem[] driftSmoke;

    void Start()
    {

    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckIfDrifting();
    }

    void CheckIfDrifting()
    {
        if (kc.drifting && kc.grounded && kc.speed > 10)
            StartSmoke();
    }

    void StartSmoke()
    {
        for (int i = 0; i < driftSmoke.Length; i++)
            driftSmoke[i].Play();
    }
}