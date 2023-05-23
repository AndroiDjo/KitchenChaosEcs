using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private AudioSource audioSource;

    private const string PREFS_MUSIC_VOLUME = "PrefsMusicVolume";
    public static MusicManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat(PREFS_MUSIC_VOLUME, 0.3f);
    }

    public float GetVolume() {
        return audioSource.volume;
    }

    public void SetVolume(float volume) {
        if (volume > 1f) {
            volume = 1f;
        } else if (volume < 0f) {
            volume = 0f;
        }

        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

}
