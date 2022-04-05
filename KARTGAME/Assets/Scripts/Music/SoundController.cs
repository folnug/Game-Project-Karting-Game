using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class SoundController
{

    public enum Sound
    {
        // KART //
        KartHop,
        KartDrift,
        KartBoost,

        // TRACK //
        TrackCheckpoint,
        TrackCountdown,
    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void Intialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.KartDrift] = 0f;
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject = null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource= oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
        
    }

    public static void PlaySound(Sound sound, Vector3 position, float volume)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);

            audioSource.volume = volume;

            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;

            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }


    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.KartDrift:
                SoundTimer(sound, .05f);
                return true;
            case Sound.KartBoost:
                SoundTimer(sound, .05f);
                return true;
            case Sound.KartHop:
                return true;
            case Sound.TrackCheckpoint:
                return true;
            case Sound.TrackCountdown:
                SoundTimer(sound, .05f);
                return true;
            default:
                return true;
        }
    }

    private static bool SoundTimer(Sound sound, float kartDriftTimerMax)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            float lastTimePlayed = soundTimerDictionary[sound];
            if (lastTimePlayed + kartDriftTimerMax < Time.time)
            {
                soundTimerDictionary[sound] = Time.time;
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return (soundAudioClip.audioClip);
            }
        }
        Debug.LogWarning($"SoundController: GetAudioClip() | Sound: {sound} not found.");
        return null;    
    }

    public static void PlayAudio(AudioSource audioSource, AudioClip[] audioList, float volume)
    {
        //Debug.Log($"SoundController PlayAudio() | AudioList: {audioList} played. Has {audioList.Length} in list.");
        int toPlay = Random.Range(0, audioList.Length);
        audioSource.PlayOneShot(audioList[toPlay], volume);
        audioSource.Play(); 
    }
}
