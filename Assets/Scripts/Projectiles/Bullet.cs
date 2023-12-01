using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 2.0f;
    public Rigidbody rb;
    public ParticleSystem explosionParticleSystem;
    public float explosionTime;
    public GameObject model;

    public float lifeSpan;
    public float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        explosionTime = explosionParticleSystem.main.duration;
    }

    public virtual void OnEnable()
    {
        AddMovement();

        lifeTimer = 0;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeSpan)
        {
            StartCoroutine(PlayExplosionParticles());
        }

    }

    public void AddMovement()
    {
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "AR_Plane")
        {
            return;
        }

        if (other.gameObject.tag == "AR_Player")
        {
            AR_GameManager.instance.playerScript.Damage();
        }

        // Stop the object and play destroy effect
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(PlayExplosionParticles());
    }

    public IEnumerator PlayExplosionParticles()
    {
        model.SetActive(false);

        explosionParticleSystem.Play();
        
        yield return new WaitForSeconds(explosionTime);

        explosionParticleSystem.Stop();
        gameObject.SetActive(false);
        model.SetActive(true);

        SaveBullet();
    }

    public virtual void SaveBullet()
    {

    }
}
