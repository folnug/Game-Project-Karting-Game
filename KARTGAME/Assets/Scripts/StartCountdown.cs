using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            kartControllers = FindObjectsOfType<KartController>();
            foreach(KartController kartController in kartControllers) kartController.SetState(KartController.KartStates.Drive);
            duration = 0f;
        }
    }

    public float GetCountdown() => duration;

    public string GetCountdownString() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        return timeSpan.ToString("%s");
    }
}
