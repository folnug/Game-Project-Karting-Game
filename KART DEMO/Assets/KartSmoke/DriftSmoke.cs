using UnityEngine;
using System.Collections;
using System;

public class DriftSmoke : MonoBehaviour
{
    [SerializeField] KartController kc;
    [SerializeField] ParticleSystem[] smoke;

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
        //if (Input.GetKey(KeyCode.LeftShift)) DEBUG
        if (kc.drifting && kc.grounded && kc.speed > 10)
        {
            StartSmoke();
        }
    }

    void StartSmoke()
    {
        for (int i = 0; i < smoke.Length; i++)
        {
            smoke[i].Play();
        }

    }

    void StopSmoke()
    {
        for (int i = 0; i < smoke.Length; i++)
        {
            smoke[i].Stop();
        }
    }
}