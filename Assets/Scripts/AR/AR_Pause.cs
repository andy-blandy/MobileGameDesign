using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AR_Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject loseScreen;
    public GameObject winScreen;

    public static AR_Pause instance;
    void Awake()
    {
        instance = this;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void SwitchScene(int sceneNumber)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNumber);
    }
}
