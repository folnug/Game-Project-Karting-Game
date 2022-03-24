using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUi : MonoBehaviour
{
    [SerializeField] Text txt;
    [SerializeField] KartController ks;

    void Update() {
        txt.text = ks.speed.ToString();
    }
}
