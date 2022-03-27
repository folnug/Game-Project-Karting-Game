using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem[] boostEffects;
    void OnEnable() {
       KartController.BoostUsed += ActivateBoostEffect;
    }

    void ActivateBoostEffect() {
        foreach(ParticleSystem boostEffect in boostEffects) boostEffect.Play();
    }
}
