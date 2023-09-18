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
        if (other.gameObject.tag == "TutorialTrigger")
        {
            Tutorial.instance.CompleteLevelPiece();
            return;
        }

        gameManager.PlayerIsHit();
    }
}
