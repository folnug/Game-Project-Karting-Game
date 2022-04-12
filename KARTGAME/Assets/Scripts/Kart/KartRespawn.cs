using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartRespawn : MonoBehaviour
{
    [SerializeField] Transform sphere;
    [SerializeField] Transform kart;
    [SerializeField] float kartRespawnValue;

    void Update() {
        if (sphere == null && kart == null) return;

        if (sphere.position.y < kartRespawnValue) {
            sphere.position = transform.position;
            kart.rotation = transform.rotation;
        }
    }
}
