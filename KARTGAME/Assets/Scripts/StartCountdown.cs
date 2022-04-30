using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] float duration = 0f;
    public static event Action CountdownComplete;
    public static event Action<float> UpdateUIDuration;

    bool runCounter = false;

    void OnEnable() {
        TrackManager.StartCounter += StartCounter;
    }

    void OnDisable() {
        TrackManager.StartCounter -= StartCounter;
    }

    void StartCounter() {
        Debug.Log("Run countdown");
        runCounter = true;
    }

    void Update()
    {
        if (!runCounter) return;
        duration -= Time.deltaTime;
        UpdateUIDuration?.Invoke(duration);
        if (duration <= 0f) {
            runCounter = false;
            duration = 0f;
            CountdownComplete?.Invoke();
        }
    }
}
