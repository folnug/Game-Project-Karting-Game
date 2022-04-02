using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    //Audio
    //public AudioSource audioSource;
    //public AudioClip[] hopAudioList;

    //AudioSource + AudioClip
    public static void PlayAudio(AudioSource audioSource, AudioClip[] audioList)
    {
        int toPlay = Random.Range(0, audioList.Length);
        audioSource.PlayOneShot(audioList[toPlay], 0.2F);
        audioSource.Play();
    }
}
