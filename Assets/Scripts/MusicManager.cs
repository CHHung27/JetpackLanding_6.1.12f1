using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }  // singleton pattern

    private const int MUSIC_VOLUME_MAX = 10;

    private static float musicTime;  // keeps track of music playtime so music can be synced on reload
    private static int musicVolume = 4;

    private AudioSource musicAudioSource;

    public event EventHandler OnMusicVolumeChanged;

    private void Awake()
    {
        Instance = this;

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
        musicAudioSource.volume = GetMusicVolumeNormalized();
    }

    private void Update()
    {
        musicTime = musicAudioSource.time;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }

    /// <summary>
    /// returns music volume as a percentage (float) of the volume max
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }
}
