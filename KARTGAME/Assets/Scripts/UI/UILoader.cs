using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    void Awake() {
        if (SceneManager.GetSceneByName("GameUI").isLoaded == false) {
            SceneManager.LoadSceneAsync("GameUI", LoadSceneMode.Additive);
            Debug.Log("Load UI");
        }
    }
}
