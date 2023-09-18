using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.instance;    
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT " + other.name);
        gameManager.SpawnPlayer();
    }
}
