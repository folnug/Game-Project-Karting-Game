using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] karts;
    [SerializeField] float gap = 20f;
    void Awake() {
        Vector3 pos = new Vector3(0, 0, 0);
        for(int i = 0; i < karts.Length; i++) {
            pos = new Vector3(i * gap, 0, 0);
            Instantiate(karts[i], pos + transform.position, transform.rotation);
        }
    }

    void OnDrawGizmos() {
        Vector3 pos = new Vector3(0, 0, 0);
        for(int i = 0; i < karts.Length; i++) {
            pos = new Vector3(i * gap, 0, 0);
            Gizmos.DrawCube(pos + transform.position, new Vector3(2, 2, 2));
        }
    }

}
