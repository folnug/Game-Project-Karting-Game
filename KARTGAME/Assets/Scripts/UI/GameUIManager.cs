using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{

    MainControls mainControls;
    [SerializeField] string mainMenuScene = "CharacterTrackSelectTest";

    bool toggle = false;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameMenu;

    void Awake() {
        mainControls = new MainControls();
        mainControls.Menu.Enable();
    }

    void OnEnable() {
        mainControls.Menu.ToggleMenu.started += ToggleMenu;
    }

    void OnDisable() {
        mainControls.Menu.ToggleMenu.started -= ToggleMenu;
    }


    void ToggleMenu(InputAction.CallbackContext context) {
        toggle = !toggle;
        UpdateUI();
    }

    public void QuitGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ContinueGame() {
        toggle = !toggle;
        UpdateUI();
    }

    void UpdateUI() {
        pauseMenu.SetActive(toggle);
        gameMenu.SetActive(!toggle);

        if (toggle) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
