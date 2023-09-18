using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public List<TextMeshProUGUI> levelButtonsText;

    private List<string> levelNames;

    void Start()
    {
        OpenFolder();
    }

    void OpenFolder()
    {
        try
        {
            // Instantiate lists
            List<string> namesOfScenesInBuild = new List<string>();
            levelNames = new List<string>();
            List<EditorBuildSettingsScene> newSettings = new List<EditorBuildSettingsScene>();

            // Get the locations of all the level files
            string levelFolderLocation = "Assets/Scenes/Levels";
            string[] fileNames = Directory.GetFiles(levelFolderLocation);

            // Get all of the existing scenes in the build
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                newSettings.Add(scene);
                namesOfScenesInBuild.Add(scene.path);
            }

            foreach (string fileName in fileNames)
            {
                // Only focus on the files with the ".unity" file type
                string fileType = fileName.Substring(fileName.Length - 5, 5);
                if (!fileType.Equals("unity"))
                {
                    continue;
                }

                // Add the level to the build settings (if it's not already there)
                string newFileName = fileName.Replace(@"\", "/");
                if (!namesOfScenesInBuild.Contains(newFileName))
                {
                    newSettings.Add(new EditorBuildSettingsScene(newFileName, true));
                }

                // Format string to be displayed
                newFileName = newFileName.Replace(levelFolderLocation, "").Replace("/", "").Replace(".unity", "");
                levelNames.Add(newFileName);
            }

            // Update build settings with new list of levels
            EditorBuildSettings.scenes = newSettings.ToArray();
            UpdateLevelList();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Update the text of buttons in the current scene with the level names
    /// </summary>
    void UpdateLevelList()
    {
        for (int i = 0; i < levelButtonsText.Count; i++)
        {
            if (i < levelNames.Count)
            {
                levelButtonsText[i].text = levelNames[i].ToString();
            } else
            {
                levelButtonsText[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void LoadLevel(int buttonNumber)
    {
        SceneManager.LoadScene(levelButtonsText[buttonNumber].text);
    }

    public void LoadLevelWithText(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
