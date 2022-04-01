using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Checkpoint : MonoBehaviour
{
    private bool isTriggered = false;

    //Objekti jolla on rigidbody osuu t�h�n
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sphere")
        {

            if (isTriggered)
                return;

            //L�hett�� viestin Goalille nimell� Checkpoint ja disablee itsens�
            GameObject.Find("Goal").SendMessage("Checkpoint");
            isTriggered = true;
            Debug.Log("Checkpoint");
        }
    }

    //Vastaanottaa ottaa CheckpointReset viestin
    //resettaa checkpointit ja goalin
    void CheckpointReset()
    {
        isTriggered = false;
        Debug.Log("Sain reset viestin");
    }
}

