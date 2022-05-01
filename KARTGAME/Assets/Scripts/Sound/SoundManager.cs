using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class SoundManager
{
    //private static string[] audioSourcePitchArray = {"Kart"};

    public enum Sound
    {
        // KART //
        KartHop,
        KartDrift,
        KartBoost,

        // TRACK //
        TrackCheckpoint,
        TrackCountdown,

        // MENU (ButtonSounds etc.) //
        MenuButton,
        MenuConfirmButton,

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
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
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

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.maxDistance = 50f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;

            audioSource.Play();

            UnityEngine.Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySoundMenu(Sound sound, float volume, bool checkPitch)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);

            audioSource.volume = volume;
            if (!checkPitch)
                audioSource.pitch = Random.Range(0.95f, 1.05f);

            audioSource.rolloffMode = AudioRolloffMode.Linear;

            audioSource.Play();

            UnityEngine.Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.KartDrift:
                if (soundTimerDictionary.ContainsKey(sound)) 
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerDriftTimerMax = .05f; //T�M�
                    if (lastTimePlayed + playerDriftTimerMax < Time.time) {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogWarning($"SoundController: GetAudioClip() | Sound: {sound} not found.");
        return null;    
    }

    public static void AddButtonSounds() { }
}
