using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartBoostDurationBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] KartController kc;
    void Start() {
        slider.maxValue = 2f;
    }

    void FixedUpdate() {
        slider.value = kc.currentBoostTime;
    }
}
