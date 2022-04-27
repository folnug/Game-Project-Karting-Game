using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{

    MainControls mainControls;

    [SerializeField] string mainMenuScene = "CharacterTrackSelectTest";

    enum States {
        game,
        pause,
        stats
    }

    States currentState;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject statsMenu;

    void Awake() {
        mainControls = new MainControls();
        mainControls.Menu.Enable();

        currentState = States.game;
    }

    void OnEnable() {
        mainControls.Menu.ToggleMenu.started += ToggleMenu;
    }

    void OnDisable() {
        mainControls.Menu.ToggleMenu.started -= ToggleMenu;
    }


    void ToggleMenu(InputAction.CallbackContext context) {
        if (currentState == States.stats) return;
        if (currentState != States.pause) currentState = States.pause;
        else currentState = States.game;
        UpdateUI();
    }

    public void QuitGame() {
        Debug.Log("Quit game!");
        StartTime();
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ContinueGame() {
        currentState = States.game;
        UpdateUI();
    }

    public void GameOver() {
        currentState = States.stats;
        UpdateUI();
    }

    void UpdateUI() {
        switch(currentState) {
            case States.game:
                Game();
                break;
            case States.pause:
                Pause();
                break;
            case States.stats:
                Stats();
                break;
        }
    }

    void Pause() {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(true);
        statsMenu.SetActive(false);
        StopTime();
    }

    void Game() {
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        statsMenu.SetActive(false);
        StartTime();
    }

    void Stats() {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        statsMenu.SetActive(true);
        StartTime();
    }

    void StopTime() => Time.timeScale = 0;
    void StartTime() => Time.timeScale = 1;
}
