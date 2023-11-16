using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Player : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public void Start()
    {
        currentHealth = maxHealth;

        string playerCurrentHealth = "Current HP: " + currentHealth.ToString() + "/" + maxHealth.ToString();
        AR_GameManager.instance.SetDebugText(playerCurrentHealth);
    }

    public void Damage()
    {
        AR_GameManager.instance.SetDebugText("HIT");

        currentHealth--;

        string playerCurrentHealth = "Current HP: " + currentHealth.ToString() + "/" + maxHealth.ToString();
        AR_GameManager.instance.SetDebugText(playerCurrentHealth);

        if (currentHealth <= 0)
        {
            AR_GameManager.instance.GameOver();
        }
    }
}
