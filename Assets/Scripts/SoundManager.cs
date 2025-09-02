using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SOUND_VOLUME_MAX = 10;
    private static int soundVolume = 6;
    private static bool isInitialSetup = true;

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip coinPickUpAudioClip;
    [SerializeField] private AudioClip fuelPickUpAudioClip;
    [SerializeField] private AudioClip landingCrashAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;

    public event EventHandler OnSoundVolumeChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // sets sound parameter to the same as music parameter; but only if it's the
        // first time entering game scene
        if (isInitialSetup) soundVolume = MusicManager.Instance.GetMusicVolume();
        isInitialSetup = false;

        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnCoinPickup += Lancer_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    #region Play Sound Events
    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                // plays success sound right on top of the camera; since this is a 2D game
                AudioSource.PlayClipAtPoint(landingSuccessAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
                break;
            default:
                // plays crash sound right on top of the camera; since this is a 2D game
                AudioSource.PlayClipAtPoint(landingCrashAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
                break;

        }
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        // plays fuel pickup sound right on top of the camera; since this is a 2D game
        AudioSource.PlayClipAtPoint(fuelPickUpAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    private void Lancer_OnCoinPickup(object sender, System.EventArgs e)
    {
        // plays coin pickup sound right on top of the camera; since this is a 2D game
        AudioSource.PlayClipAtPoint(coinPickUpAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    } 
    #endregion

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume()
    {
        return soundVolume;
    }

    public float GetSoundVolumeNormalized()
    {
        return ((float) soundVolume) / SOUND_VOLUME_MAX;
    }
}
