using UnityEngine;
using Cinemachine;

public class LookBehind : MonoBehaviour
{
    CinemachineVirtualCamera cVirCam;
    CinemachineTransposer cT;

    bool toggle = false;

    void Start()
    {
        cVirCam = GetComponent<CinemachineVirtualCamera>();
        cT = cVirCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    void OnEnable() {
        TrackManager.EndRace += LockBehindCamera;
    }

    void OnDisable() {
        TrackManager.EndRace -= LockBehindCamera;
    }

    void FixedUpdate()
    {
        if (toggle) return;
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.JoystickButton4))
            cT.m_FollowOffset.z = 10.73f;
        
        else
            cT.m_FollowOffset.z = -10.73f;
    }

    void LockBehindCamera() {
        cT.m_FollowOffset.z = 10.73f;
        toggle = true;
    }
}
