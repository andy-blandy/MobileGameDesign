using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public AudioSource attackSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            attackSound.Play();
        }

        if (other.gameObject.tag == "Deflect")
        {
            other.gameObject.GetComponent<DeflectableBullet>().Deflect();
        }
    }
}
