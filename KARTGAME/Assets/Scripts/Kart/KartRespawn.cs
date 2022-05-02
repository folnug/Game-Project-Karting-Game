using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartRespawn : MonoBehaviour
{
    void OnEnable() {
        KartController.RespawnKart += RespawnKart;
    }

    void OnDisable() {
        KartController.RespawnKart -= RespawnKart;
    }

    
    public void RespawnKart(Transform kart, Transform motor) {
        KartCheckpointData data = kart.transform.GetComponent<KartCheckpointData>();
        if (data != null) {
            motor.position = data.nextCheckpoint.transform.position;
            Quaternion rot = Quaternion.LookRotation(data.nextCheckpoint.transform.position - kart.transform.position);
            kart.transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
        }
    }
}
