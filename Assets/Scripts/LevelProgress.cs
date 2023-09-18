using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelProgress : MonoBehaviour
{
    int score = 0;
    float time = 0f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI progressBar;

    // Start is called before the first frame update
    void Start()
    {
        setScoreText();
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
        scoreText.text = "Count: " + score.ToString();
    }

    
}
