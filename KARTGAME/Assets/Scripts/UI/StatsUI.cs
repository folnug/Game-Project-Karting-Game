using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform container;
    CheckpointHandler checkpointHandler; 
    void Awake()
    {
        checkpointHandler = FindObjectOfType<CheckpointHandler>();
    }

    public void OnEnable() {
        List<KartCheckpointData> karts = checkpointHandler.GetKarts();
        foreach(KartCheckpointData kart in karts) {
            GameObject temp = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(container);
            PositionUI position = temp.transform.GetComponent<PositionUI>();
            position.SetValues(kart.position.ToString(), kart.GetKartName());
        }
    }
}
