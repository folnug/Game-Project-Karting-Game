using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splashscreen : MonoBehaviour
{
    public GameObject splash;
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            splash.SetActive(false);
            menu.SetActive(true);
        }
    }
}
