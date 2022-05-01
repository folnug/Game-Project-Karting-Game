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
    [SerializeField] GameObject driftModeSelect;
    [SerializeField] GameObject trackSelect;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject trackPrefab;
    [SerializeField] GameObject characterContainer;
    [SerializeField] GameObject driftContainer;
    [SerializeField] GameObject trackContainer;

    EventSystem eventSystem;

    enum UIStates {
        CharacterSelection,
        DriftModeSelection,
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
        trackSelect.SetActive(false);
        driftModeSelect.SetActive(false);
        characterSelect.SetActive(false);
        
        switch(currentState) {
            case UIStates.CharacterSelection:
                characterSelect.SetActive(true);
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(characterContainer.transform.GetChild(0).gameObject);
                break;
            case UIStates.DriftModeSelection:
                driftModeSelect.SetActive(true);
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(driftContainer.transform.GetChild(0).gameObject);
                break;
            case UIStates.TrackSelection:
                trackSelect.SetActive(true);
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(trackContainer.transform.GetChild(0).gameObject);
                break;
        }
    }

    public void SetDriftMode(int mode) {
        KartController.KartDriftModes driftMode = (KartController.KartDriftModes)mode;
        characterSelection.playerDriftMode = driftMode;
        currentState = UIStates.TrackSelection;
        SoundManager.PlaySoundMenu(SoundManager.Sound.MenuButton, 0.05f, false);
        SwitchSelection();
    }

    public void BackButton() {
        currentState = lastState;
        SoundManager.PlaySoundMenu(SoundManager.Sound.MenuButton, 0.05f, true);
        SwitchSelection();
    }

    public void CharacterSelected(int index) {
        characterSelection.playerCharacterIndex = index;
        lastState = currentState;
        currentState = UIStates.DriftModeSelection;
        SoundManager.PlaySoundMenu(SoundManager.Sound.MenuButton, 0.05f, true);
        SwitchSelection();
    }

    public void TrackSelected(int index) {
        Track selected = trackPool.tracks[index];
        SoundManager.PlaySoundMenu(SoundManager.Sound.MenuConfirmButton, 0.05f, true);
        SceneManager.LoadScene(selected.trackScene);
    }
}
