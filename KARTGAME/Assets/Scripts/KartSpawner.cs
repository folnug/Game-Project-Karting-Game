using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class KartSpawner : MonoBehaviour
{
    
    [SerializeField] float gap = 20f;

    [SerializeField] CinemachineVirtualCamera vcam;

    public static event Action<KartController[]> KartSpawnComplete;
    
    void OnEnable() {
        TrackManager.SpawnKarts += SpawnKarts;
    }

    void OnDisable() {
        TrackManager.SpawnKarts -= SpawnKarts;
    }

    void SpawnKarts(CharacterSelection characterSelection) {
        KartController[] karts = new KartController[characterSelection.characters.Length];
        Vector3 pos = new Vector3(0, 0, 0);
        for(int i = 0; i < characterSelection.characters.Length; i++) {
            KartModel character = characterSelection.characters[i];
            pos = new Vector3(i * gap, 0, 0);
            GameObject kart = Instantiate(character.kart, pos + transform.position, transform.rotation);
            KartController kartController =  kart.GetComponent<KartController>();
            if (i == characterSelection.playerCharacterIndex) {
                kart.AddComponent<PlayerInput>();
                kartController.driftMode = characterSelection.playerDriftMode;
                vcam.Follow = kart.transform;
                vcam.LookAt = kart.transform;
            } else {
                kart.AddComponent<AiInput>();
            }
            karts[i] = kartController;
        }
        KartSpawnComplete?.Invoke(karts);
        Debug.Log("Karts spawned");
    }
}
