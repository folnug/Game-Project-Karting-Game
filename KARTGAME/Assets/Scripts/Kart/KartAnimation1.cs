using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartAnimation1 : MonoBehaviour
{
    Animator anim;
    KartController kartController;
    void Start()
    {
        anim = GetComponent<Animator>();
        kartController = GetComponent<KartController>();
    }

    void OnEnable() {
        TrackManager.WinnerKart += WonOrLost;
    }

    void OnDisable() {
        TrackManager.WinnerKart -= WonOrLost;
    }

    void WonOrLost(Transform kart) {
        if (transform == kart) {
            anim.SetBool("Victory", true);
        } else {
            anim.SetBool("Loss", true);
        }
    }

}
