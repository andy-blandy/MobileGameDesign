using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AR_GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI debugText;


    public AR_Player playerScript;

    public static AR_GameManager instance;

    void Awake()
    {
        instance = this;
    }
    public void GameOver()
    {

    }

    public void SetDebugText(string incomingText)
    {
        debugText.text = incomingText;
    }
}
