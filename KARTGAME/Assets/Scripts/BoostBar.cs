using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Awake() {
        slider.maxValue = 100;
    }

    void OnEnable() {
       KartController.UpdateBoostUi += UpdateUI;
    }

    void OnDisable() {
       KartController.UpdateBoostUi -= UpdateUI;
    }

    void UpdateUI(float ammount) {
        slider.value = ammount;
    }
}
