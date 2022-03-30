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
            if (kc.drifting && kc.grounded && kc.currentSpeed > 20)
                skidMarks[i].emitting = true;
            else
                skidMarks[i].emitting = false;
    }
}