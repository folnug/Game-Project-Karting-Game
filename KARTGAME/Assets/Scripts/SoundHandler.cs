using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SoundHandler : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.Intialize();
    }
}
