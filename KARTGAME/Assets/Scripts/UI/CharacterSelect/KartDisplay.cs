using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartDisplay : MonoBehaviour
{
    int index;
    public KartModel kart;
    CharacterTrackSelectManager manager;
    [SerializeField] Text kartName;
    [SerializeField] Image kartImage;

    public void UpdateDisplay() {
        kartName.text = kart.driverName;
        kartImage.sprite = kart.image;
    }

    public void SetValues(int index, CharacterTrackSelectManager manager) {
        this.index = index;
        this.manager = manager;
    }
}
