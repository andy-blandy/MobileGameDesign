using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelProgress : MonoBehaviour
{
    int score = 0;
    float time = 0f;
    public int deaths = 0;
    public bool speed = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI deathText;

    #region Singleton
    public static LevelProgress instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        setScoreText();
        setDeathText();
        setSpeedText();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelText.alpha > 0f)
        {
            levelText.alpha = levelText.alpha - 0.005f;
        }

        time = time + 0.5f;
        if (time % 125 == 0)
        {
            score = score + 5;
        }
        setScoreText();
    }


    public void setScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void setDeathText()
    {
        deathText.text = "KIA: " + deaths.ToString();
    }

    public void setSpeedText()
    {
        string diff = "";
        if (speed == true)
        {
            diff = "HARD";
        }
        else
        {
            diff = "EASY";
        }
        speedText.text = "Speed: " + diff;
    }


}