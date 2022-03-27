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
        controls.Kart.Drifting.performed += StartDrifting;
        controls.Kart.Drifting.canceled += StopDrifting;
    }

    void OnDisable() {
        controls.Kart.Hop.started -= StartHop;
        controls.Kart.Drifting.performed -= StartDrifting;
        controls.Kart.Drifting.canceled -= StopDrifting;
    }

    void Update() {
        horizontal = controls.Kart.Horizontal.ReadValue<float>();
        vertical = controls.Kart.Vertical.ReadValue<float>();

        kc.SetInputs(horizontal, vertical);
    }

    void StartHop(InputAction.CallbackContext context) => kc.Hop();

    void StartDrifting(InputAction.CallbackContext context) => kc.drifting = true;
    void StopDrifting(InputAction.CallbackContext context) => kc.drifting = false;

} 
