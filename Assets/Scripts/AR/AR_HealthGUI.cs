using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_HealthGUI : MonoBehaviour
{
    public GameObject[] healthBars;

    public static AR_HealthGUI instance;
    void Awake()
    {
        instance = this;
    }

    public void SetHealth(int health)
    {
        for (int i = 0; i < health; i++)
        {
            if (healthBars[i].activeSelf)
            {
                continue;
            }

            healthBars[i].SetActive(true);
        }

        for (int i = health; i < healthBars.Length; i++)
        {
            if (!healthBars[i].activeSelf)
            {
                break;
            }

            healthBars[i].SetActive(false);
        }
    }
}
