using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KartSpawner : MonoBehaviour
{
    [SerializeField] CharacterSelection characterSelection;
    [SerializeField] float gap = 20f;

    [SerializeField] CinemachineVirtualCamera vcam;

    void Awake() {
        Vector3 pos = new Vector3(0, 0, 0);
        for(int i = 0; i < characterSelection.characters.Length; i++) {
            KartModel character = characterSelection.characters[i];
            pos = new Vector3(i * gap, 0, 0);
            GameObject kart = Instantiate(character.kart, pos + transform.position, transform.rotation);
            if (i == characterSelection.playerCharacterIndex) {
                kart.AddComponent<PlayerInput>();
                vcam.Follow = kart.transform;
                vcam.LookAt = kart.transform;
            } else {
                kart.AddComponent<AiInput>();
            }
        }
    }
    void OnDrawGizmos() {
        Vector3 pos = new Vector3(0, 0, 0);
        for(int i = 0; i < characterSelection.characters.Length; i++) {
            pos = new Vector3(i * gap, 0, 0);
            Gizmos.DrawCube(pos + transform.position, new Vector3(2, 2, 2));
        }
    }
}
