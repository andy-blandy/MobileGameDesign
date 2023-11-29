using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AR_GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI debugText;

    [Header("VFX")]
    public GameObject smokeEffect;

    [Header("Player")]
    public AR_Player playerScript;

    public static AR_GameManager instance;

    void Awake()
    {
        instance = this;
    }

    public void LoseGame()
    {
        Time.timeScale = 0f;
        AR_Pause.instance.loseScreen.SetActive(true);
    }

    public void WinGame()
    {
        Time.timeScale = 0f;
        AR_Pause.instance.winScreen.SetActive(true);
    }

    public void SetDebugText(string incomingText)
    {
        debugText.text = incomingText;
    }
}
