using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] Ghost ghost;
    float time = 0f;
    int index1;
    int index2;
    bool runReplay = false;

    GameObject character;

    public static Action<Ghost> GhostData;

    void OnEnable() {
        TrackManager.ReplayGhost += StartReplay;
    }

    void OnDisable() {
        TrackManager.ReplayGhost -= StartReplay;
    }

    void Awake() {
        character = Instantiate(ghost.character, transform.position, transform.rotation);
        character.SetActive(false);
    }

    void StartReplay() {
        runReplay = true;
        character.SetActive(true);
        GhostData?.Invoke(ghost);
    }

    void Update() {
        if (!runReplay) return;

        time += Time.unscaledDeltaTime;

        if (ghost.replay & index1 < ghost.timestamp.Count) {
            GetIndex();
            SetTransform();
        }
    }

    void GetIndex() {
        for (int i = 0; i < ghost.timestamp.Count - 2; i++) {
            if (ghost.timestamp[i] == time) {
                index1 = i;
                index2 = i;
                return;
            } else if (ghost.timestamp[i] < time & time < ghost.timestamp[i + 1]) {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = ghost.timestamp.Count - 1;
        index2 = ghost.timestamp.Count - 1;
    }

    void SetTransform() {
        if (index1 == index2) {
            character.transform.position = ghost.position[index1];
            character.transform.eulerAngles = ghost.rotation[index1];
        } else {
            float factor = ((time - ghost.timestamp[index1]) / (ghost.timestamp[index2] - ghost.timestamp[index1]));
            character.transform.position = Vector3.Lerp(ghost.position[index1], ghost.position[index2], factor);
            character.transform.eulerAngles = Vector3.Lerp(ghost.rotation[index1], ghost.rotation[index2], factor);
        }
    }
}
