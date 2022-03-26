using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartRespawn : MonoBehaviour
{
    [SerializeField] Transform kart;

    void Update() {
        if (kart == null) return;

        if (kart.position.y < -5f) {
            kart.position = transform.position;
        }
    }
}
