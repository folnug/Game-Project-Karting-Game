using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    EventSystem eventSystem;

    enum UIStates {
        CharacterSelection,
        TrackSelection
    }

    UIStates currentState;
    UIStates lastState;

    void Start()
    {
        currentState = UIStates.CharacterSelection;
        lastState = currentState;
        CreateCharacterSelection();
        CreateTrackSelection();
        eventSystem = EventSystem.current;
        SwitchSelection();
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

    void SwitchSelection() {
        switch(currentState) {
            case UIStates.CharacterSelection:
                characterSelect.SetActive(true);
                trackSelect.SetActive(!characterSelect.activeSelf);
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(characterContainer.transform.GetChild(0).gameObject);
                break;
            case UIStates.TrackSelection:
                trackSelect.SetActive(true);
                characterSelect.SetActive(!trackSelect.activeSelf);
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(trackContainer.transform.GetChild(0).gameObject);
                break;
        }
    }

    public void BackButton() {
        currentState = lastState;
        SwitchSelection();
    }

    public void CharacterSelected(int index) {
        characterSelection.playerCharacterIndex = index;
        lastState = currentState;
        currentState = UIStates.TrackSelection;
        SwitchSelection();
    }

    public void TrackSelected(int index) {
        Track selected = trackPool.tracks[index];
        SceneManager.LoadScene(selected.trackScene);
    }
}
