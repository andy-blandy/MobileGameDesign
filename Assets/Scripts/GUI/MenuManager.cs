using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void StopMusic()
    {
        MenuMusic.instance.KillMusic();
    }

    public void SetDifficulty(string difficultyType)
    {
        PlayerPrefs.SetString("Difficulty", difficultyType);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }


}
