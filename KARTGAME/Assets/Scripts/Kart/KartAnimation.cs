using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] KartController kc;

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", kc.speed);

        Debug.Log(kc.speed);
    }
}
