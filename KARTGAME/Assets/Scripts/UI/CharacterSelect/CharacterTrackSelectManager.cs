using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterTrackSelectManager : MonoBehaviour
{
    [SerializeField] CharacterSelection characterSelection;
    [SerializeField] TrackPool trackPool;
    [SerializeField] GameObject characterSelect;
    [SerializeField] GameObject trackSelect;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject trackPrefab;
    [SerializeField] GameObject characterContainer;
    [SerializeField] GameObject trackContainer;

    void Start()
    {
        CreateCharacterSelection();
        CreateTrackSelection();
    }
    
    void CreateCharacterSelection() {
        for (int i = 0; i < characterSelection.characters.Length; i++) {
            KartModel character = characterSelection.characters[i];
            GameObject temp = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(characterContainer.transform);
            KartDisplay characterDisplay = temp.GetComponent<KartDisplay>();
            characterDisplay.kart = character;
            characterDisplay.UpdateDisplay();
            characterDisplay.SetValues(i, this);
        }
    }

    void CreateTrackSelection() {
        for (int i = 0; i < trackPool.tracks.Length; i++) {
            GameObject temp = Instantiate(trackPrefab, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(trackContainer.transform);
            TrackDisplay trackDisplay = temp.GetComponent<TrackDisplay>();
            trackDisplay.track = trackPool.tracks[i];
            Debug.Log(trackPool.tracks[i]);
            trackDisplay.UpdateDisplay();
            trackDisplay.SetValues(i, this);
        }
    }

    public void CharacterSelected(int index) {
        characterSelection.playerCharacterIndex = index;
        TrackSelectSetActive(true);
    }

    public void TrackSelected(int index) {
        Track selected = trackPool.tracks[index];
        SceneManager.LoadScene(selected.trackScene);
    }
    
    void CharacterSelectSetActive(bool value) {
        characterSelect.SetActive(value);
        trackSelect.SetActive(!value);
    }

    void TrackSelectSetActive(bool value) {
        trackSelect.SetActive(value);
        characterSelect.SetActive(!value);
    }
}
