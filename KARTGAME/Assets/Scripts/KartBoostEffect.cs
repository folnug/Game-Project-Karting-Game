using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartBoostEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] KartController kc;
    void Update()
    {
        if (!kc.drifting && kc.driftValue > 100f * 0.8f) {
            foreach(ParticleSystem particle in particles) particle.Play();
        }
    }
}