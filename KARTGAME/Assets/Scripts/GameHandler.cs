using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] confirmAudioList;
    public int confirmRangeScan;
    private int toPlay;

    public void LoadScene(string sceneName)
    {
        //PlayAudio(confirmAudioList);
        SceneManager.LoadScene(sceneName);
    }

    /*private void PlayAudio(AudioClip[] testList)
    {
        toPlay = Random.Range(0, testList.Length);
        audioSource.PlayOneShot(testList[toPlay], 0.2F);
        audioSource.Play();
    }*/

    private void Awake()
    {
        SoundController.Intialize();
    }
}
