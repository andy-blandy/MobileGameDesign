using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static readonly string firstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SFXPref = "SFXPref";
    private int firstPlayInt;
    public float bgFloat, sfxFloat;
    public AudioSource bgAudio;
    public AudioSource[] sfxAudio;

    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(firstPlay);
        if (firstPlayInt == 0)
        {
            bgFloat = 0.25f;
            sfxFloat = 0.75f;
            PlayerPrefs.SetFloat(BackgroundPref, bgFloat);
            PlayerPrefs.SetFloat(SFXPref, sfxFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
            UpdateSound();
        }
        else
        {
            bgFloat = PlayerPrefs.GetFloat(BackgroundPref);
            sfxFloat = PlayerPrefs.GetFloat(SFXPref);
            UpdateSound();
        }
    }

    public void ChangeMusicVolume(Slider musicSlider)
    {
        PlayerPrefs.SetFloat(BackgroundPref, musicSlider.value);

        UpdateSound();
    }

    public void ChangeSFXVolume(Slider sfxSlider) 
    {
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);

        UpdateSound();
    }

    public void UpdateSound()
    {
        bgAudio.volume = PlayerPrefs.GetFloat(BackgroundPref);
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = PlayerPrefs.GetFloat(SFXPref);
        }
    }
}
