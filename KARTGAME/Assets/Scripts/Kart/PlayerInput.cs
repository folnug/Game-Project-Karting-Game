using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    MainControls controls;

    KartController kc;
    
    float horizontal, vertical;
    bool hop;

    void Awake() {
        controls = new MainControls();
        controls.Kart.Enable();
        kc = GetComponent<KartController>();
    }

    void OnEnable() {
        controls.Kart.Hop.started += StartHop;
        controls.Kart.Hop.canceled += StopDrifting;
    }

    void OnDisable() {
        controls.Kart.Hop.started -= StartHop;
    }

    void Update() {
        horizontal = controls.Kart.Horizontal.ReadValue<float>();
        vertical = controls.Kart.Vertical.ReadValue<float>();

        kc.SetInputs(horizontal, vertical);
    }

    void StartHop(InputAction.CallbackContext context) => kc.Hop();
    void StopDrifting(InputAction.CallbackContext context) => kc.StopDrifting();
} 
