using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 2.0f;
    private Rigidbody rb;
    public ParticleSystem explosionParticleSystem;
    public GameObject model;

    public float lifeSpan;
    public float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        AddMovement();

        lifeTimer = 0;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeSpan)
        {
            gameObject.SetActive(false);
            SaveBullet();
        }

    }

    public void AddMovement()
    {
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "AR_Plane")
        {
            return;
        }

        if (other.gameObject.tag == "AR_Player")
        {
            AR_GameManager.instance.playerScript.Damage();
        }

        AR_GameManager.instance.SetDebugText(other.gameObject.name);

        // Stop the object and play destroy effect
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        model.SetActive(false);
        StartCoroutine(PlayExplosionParticles());
    }

    IEnumerator PlayExplosionParticles()
    {
        explosionParticleSystem.Play();

        while (explosionParticleSystem.isPlaying)
        {
            yield return null;
        }

        gameObject.SetActive(false);
        model.SetActive(true);

        SaveBullet();
    }

    public virtual void SaveBullet()
    {

    }
}
