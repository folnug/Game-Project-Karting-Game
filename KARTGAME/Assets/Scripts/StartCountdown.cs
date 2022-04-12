using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] float duration = 0f;
    KartController[] kartControllers;

    void Awake() {
        kartControllers = FindObjectsOfType<KartController>();
    }

    void Update()
    {
        if (duration == 0f) return;
        duration -= Time.deltaTime;
        if (duration <= 0f) {
            foreach(KartController kartController in kartControllers) kartController.SetState(KartController.KartStates.Drive);
            duration = 0f;
        }
    }
}
