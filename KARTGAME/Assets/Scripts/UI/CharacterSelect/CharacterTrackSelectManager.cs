using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrackSelectManager : MonoBehaviour
{
    [SerializeField] CharacterSelection characterSelection;
    [SerializeField] GameObject characterSelect;
    [SerializeField] GameObject trackSelect;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject characterContainer;

    void Start()
    {
        CreateCharacterSelection();
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

    void Update()
    {
        
    }

    
    void ToggleBetween() {
        characterSelect.SetActive(!characterSelect.activeSelf);
        characterSelect.SetActive(!characterSelect.activeSelf);
    }
}
