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
    public Slider bgSlider, sfxSlider;
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
            bgSlider.value = bgFloat;
            sfxSlider.value = sfxFloat;
            PlayerPrefs.SetFloat(BackgroundPref, bgFloat);
            PlayerPrefs.SetFloat(SFXPref, sfxFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
            UpdateSound();
        }
        else
        {
            bgFloat = PlayerPrefs.GetFloat(BackgroundPref);
            bgSlider.value = bgFloat;
            sfxFloat = PlayerPrefs.GetFloat(SFXPref);
            sfxSlider.value = bgFloat;
            UpdateSound();
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, bgSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
    }

    private void Update()
    {
        SaveSoundSettings();
        UpdateSound();
    }

    public void UpdateSound()
    {
        bgAudio.volume = bgSlider.value;
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxSlider.value;
        }
    }
}
