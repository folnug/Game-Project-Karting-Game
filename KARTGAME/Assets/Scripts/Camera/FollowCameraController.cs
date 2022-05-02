using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCameraController : MonoBehaviour
{
    [SerializeField] InputAction action;
    Animator anim;

    enum FollowCameraState {
        Front,
        Back,
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        
        action.started += _ => SwitchCameraState(FollowCameraState.Front);
        action.canceled += _ => SwitchCameraState(FollowCameraState.Back);

        TrackManager.EndRace += EndRaceCamera;
    }

    void OnEnable() {
        action.Enable();
    }

    void OnDisable() {
        action.Disable();
    }

    void SwitchCameraState(FollowCameraState state) {
        anim.Play(state.ToString());
    }

    void EndRaceCamera() {
        anim.Play(FollowCameraState.Front.ToString());
    }
}
