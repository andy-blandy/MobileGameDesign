using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    public int currentPhase;
    public int numberOfPhases;

    public virtual void NextPhase()
    {
        Debug.Log(gameObject.name + " has not had their phases implemented");
    }

    public void Damage()
    {
        health -= 1;

        if (health <= 0)
        {
            GameManager.instance.EndLevel();
        }
    }
}
