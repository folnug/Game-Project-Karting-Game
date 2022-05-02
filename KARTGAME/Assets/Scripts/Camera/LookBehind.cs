using UnityEngine;
using Cinemachine;

public class LookBehind : MonoBehaviour
{
    CinemachineVirtualCamera cVirCam;
    CinemachineTransposer cT;

    void Start()
    {
        cVirCam = GetComponent<CinemachineVirtualCamera>();
        cT = cVirCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.JoystickButton4))
            cT.m_FollowOffset.z = 10.73f;
        
        else
            cT.m_FollowOffset.z = -10.73f;
    }

    
}
