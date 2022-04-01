using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartBoostEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] KartController kc;
    void Update()
    {
        foreach(ParticleSystem particle in particles) {
            if (kc.currentBoostTime > 0) {
                particle.Play();
            } else {
                particle.Stop();
            }
        }
    }
}