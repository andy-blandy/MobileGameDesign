using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic instance;

    void Awake()
    {
        if (MenuMusic.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            instance = this;
        }
    }

    public void KillMusic()
    {
        Destroy(gameObject);
    }
}
