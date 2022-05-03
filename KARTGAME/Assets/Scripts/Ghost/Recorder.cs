using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] Ghost ghost;
    KartController recordTarget;
    float timer;
    float time;

    bool record = false;

    void Awake() {
        if (ghost.record) {
            ghost.Reset();
            timer = 0f;
            time = 0f;
        }
    }

    void OnEnable() {
        TrackManager.SelectedKart += SetTarget;
        TrackManager.SetupRace += StartRecording;
        CheckpointHandler.TimerUpdate += TimerTime;
    }

    void OnDisable() {
        TrackManager.SelectedKart -= SetTarget;
        TrackManager.SetupRace -= StartRecording;
        CheckpointHandler.TimerUpdate -= TimerTime;
    }

    void Update()
    {
        if (!record) return;
        if (recordTarget == null) return;
        timer += Time.deltaTime;
        time += Time.unscaledDeltaTime;

        if (ghost.record & timer >= 1/ghost.recordFrequency) {
            ghost.timestamp.Add(time);
            ghost.position.Add(recordTarget.transform.position);
            ghost.rotation.Add(recordTarget.transform.eulerAngles);

            timer = 0f;
        }
    }

    void StartRecording() {
        if (ghost.record)
            record = true;
    }

    void SetTarget(KartController target) {
        recordTarget = target;
    }

    void TimerTime(float time) {
        if (ghost.record)
            ghost.totalTime = time;
    }
}
