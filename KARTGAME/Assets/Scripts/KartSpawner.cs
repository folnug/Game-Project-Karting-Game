using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class KartSpawner : MonoBehaviour
{
    
    [SerializeField] float gap = 20f;

    [SerializeField] CinemachineStateDrivenCamera vcam;

    public static event Action<KartController[]> KartSpawnComplete;
    public static event Action<Transform> PlayerKart;
    void OnEnable() {
        TrackManager.SpawnKarts += SpawnKarts;
        TrackManager.SpawnKart += SpawnKart;
    }

    void OnDisable() {
        TrackManager.SpawnKarts -= SpawnKarts;
        TrackManager.SpawnKart -= SpawnKart;
    }

    void SpawnKart(CharacterSelection characterSelection) {
        KartController[] karts = new KartController[1];
        KartModel character = characterSelection.characters[characterSelection.playerCharacterIndex];
        if (character == null) return;
        GameObject kart = Instantiate(character.kart, transform.position, transform.rotation);
        KartController kartController =  kart.GetComponent<KartController>();
        SetUpPlayerKart(kart, kartController, characterSelection);
        karts[0] = kartController;
        KartSpawnComplete?.Invoke(karts);
    }

    void SpawnKarts(CharacterSelection characterSelection) {
        KartController[] karts = new KartController[characterSelection.characters.Length];
        Vector3 pos = new Vector3(0, 0, 0);
        for(int i = 0; i < characterSelection.characters.Length; i++) {
            KartModel character = characterSelection.characters[i];
            pos = new Vector3(i * gap, 0, 0);
            GameObject kart = Instantiate(character.kart, pos + transform.position, transform.rotation);
            KartController kartController =  kart.GetComponent<KartController>();
            KartCheckpointData kartData = kart.GetComponent<KartCheckpointData>();
            kartData.maxLaps = characterSelection.maxlaps;
            if (i == characterSelection.playerCharacterIndex) {
                SetUpPlayerKart(kart, kartController, characterSelection);
            } else {
                kart.AddComponent<AiInput>();
            }
            karts[i] = kartController;
        }
        KartSpawnComplete?.Invoke(karts);
        Debug.Log("Karts spawned");
    }

    void SetUpPlayerKart(GameObject kart, KartController kartController, CharacterSelection characterSelection) {
        kart.AddComponent<PlayerInput>();
        kartController.driftMode = characterSelection.playerDriftMode;
        vcam.Follow = kart.transform;
        vcam.LookAt = kart.transform;
        PlayerKart?.Invoke(kart.transform);
    }
}
