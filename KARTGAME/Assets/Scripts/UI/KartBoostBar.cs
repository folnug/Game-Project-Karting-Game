using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartBoostBar : MonoBehaviour
{
    [SerializeField] KartController kc;
    [SerializeField] Color32 color1;
    [SerializeField] Color32 color2;
    [SerializeField] Image image;
    Slider slider;
    void Awake() {
        slider = GetComponent<Slider>();
        slider.maxValue = 100f;
    }
    void LateUpdate() {
        slider.value = kc.driftValue;
        Color color = kc.driftValue > kc.minDriftAmmount ? color2 : color1;
        image.color = color;
    }
}
