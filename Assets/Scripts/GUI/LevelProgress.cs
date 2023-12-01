using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    int score = 0;
    float time = 0f;
    public int deaths = 0;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI livesText;
    public Slider progressSlider;

    #region Singleton
    public static LevelProgress instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        SetDeathText();
    }

    void Update()
    {
        if (levelText.alpha > 0f)
        {
            levelText.alpha = levelText.alpha - 0.005f;
        }
    }

    public void SetDeathText()
    {
        deathText.text = deaths.ToString();
    }

    public void UpdateProgressSlider(float newProgressValue)
    {
        progressSlider.value = newProgressValue;
    }
}