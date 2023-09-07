using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private SpawnLevel spawnLevel;

    void Awake()
    {
        spawnLevel = SpawnLevel.instance;    
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT " + other.name);
        spawnLevel.BeginLevel();
    }
}
