using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackDisplay : MonoBehaviour
{
    int index;
    public Track track;
    Button button;
    CharacterTrackSelectManager manager;
    [SerializeField] Text trackName;
    [SerializeField] Image trackImage;

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    public void UpdateDisplay() {
        trackName.text = track.name;
        trackImage.sprite = track.trackImage;
    }

    public void SetValues(int index, CharacterTrackSelectManager manager) {
        this.index = index;
        this.manager = manager;
    }

    void ButtonClick() {
        manager.TrackSelected(index);
    }
}
