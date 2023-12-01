using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static readonly string masterPref = "MasterVol";
    private static readonly string musicPref = "MusicVol";
    private static readonly string sfxPref = "SFXVol";
    public float masterVol, musicVol, sfxVol;

    public Slider masterSlider, musicSlider, sfxSlider;

    public AudioMixer audioMixer;

    void Start()
    {
        if (!PlayerPrefs.HasKey(masterPref))
        {
            masterVol = 0f;
            musicVol = -10f;
            sfxVol = -20f;

            UpdatePlayerPrefs();
        }
        else
        {
            masterVol = PlayerPrefs.GetFloat(masterPref);
            musicVol = PlayerPrefs.GetFloat(musicPref);
            sfxVol = PlayerPrefs.GetFloat(sfxPref);
        }

        UpdateSliders();
        UpdateSound();
    }

    public void ChangeMasterVolume(Slider masterSlider)
    {
        masterVol = masterSlider.value;
        audioMixer.SetFloat("Master", masterVol);

    }

    public void ChangeMusicVolume(Slider musicSlider)
    {
        musicVol = musicSlider.value;
        audioMixer.SetFloat("Music", musicVol);
    }

    public void ChangeSFXVolume(Slider sfxSlider) 
    {
        sfxVol = sfxSlider.value;
        audioMixer.SetFloat("SFX", sfxVol);
    }

    public void ConfirmChanges()
    {
        UpdatePlayerPrefs();
        UpdateSound();
    }

    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetFloat(masterPref, masterVol);
        PlayerPrefs.SetFloat(musicPref, musicVol);
        PlayerPrefs.SetFloat(sfxPref, sfxVol);
    }

    public void UpdateSound()
    {
        audioMixer.SetFloat("Master", masterVol);
        audioMixer.SetFloat("Music", musicVol);
        audioMixer.SetFloat("SFX", sfxVol);
    }

    public void UpdateSliders()
    {
        masterSlider.value = masterVol;
        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;
    }
}
