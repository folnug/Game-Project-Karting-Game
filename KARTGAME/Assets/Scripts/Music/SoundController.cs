using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    //Template
    //public AudioSource audioSource;
    //public AudioClip[] hopAudioList;

    //AudioSource + AudioClip + [name]AudioVolume
    public static void PlayAudio(AudioSource audioSource, AudioClip[] audioList, float volume)
    {
        int toPlay = Random.Range(0, audioList.Length);
        audioSource.PlayOneShot(audioList[toPlay], volume);
        audioSource.Play();
    }
}
