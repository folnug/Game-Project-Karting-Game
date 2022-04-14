using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    Transform player;
    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        Transform player = GameObject.FindObjectOfType<PlayerInput>().transform;

        vcam.LookAt = player;
        vcam.Follow = player;
    }
}
