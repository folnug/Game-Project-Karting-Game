using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuide : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    int index = 0;

    public void Next() {
        index += 1;

        if (index >= gameObjects.Length) index = 0;

        for(int i = 0; i < gameObjects.Length; i++) {
            if (i == index) {
                gameObjects[i].SetActive(true);
            } else {
                 gameObjects[i].SetActive(false);
            }
        }

    }

}
