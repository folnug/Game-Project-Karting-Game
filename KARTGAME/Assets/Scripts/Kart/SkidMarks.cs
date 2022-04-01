using UnityEngine;
using System.Collections;
using System;

public class SkidMarks : MonoBehaviour
{
    [SerializeField] KartController kc;
    [SerializeField] TrailRenderer[] skidMarks;

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
        for (int i = 0; i < skidMarks.Length; i++)
            if (Drifting() || kc.braking)
                skidMarks[i].emitting = true;
            else
                skidMarks[i].emitting = false;
    }

    bool Drifting() => kc.drifting && kc.grounded && kc.speed > 20;
}