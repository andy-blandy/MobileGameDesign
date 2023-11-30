using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TutorialTrigger")
        {
            Tutorial.instance.CompleteLevelPiece();
            return;
        }

        StartCoroutine(PlayerMovement.instance.PlayerDeath());
    }
}
