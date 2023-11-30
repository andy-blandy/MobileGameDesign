using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectableBullet : Bullet
{
    public float deflectSpeed = 15.0f;
    public bool isDeflected;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isDeflected = false;
    }

    public void Deflect()
    {
        Vector3 rotationAmonut = new Vector3(0f, 180f, 0f);
        transform.Rotate(rotationAmonut);
        rb.AddForce(transform.forward * deflectSpeed, ForceMode.Impulse);
        isDeflected = true;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && isDeflected)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Boss" && isDeflected)
        {
            collision.gameObject.GetComponent<Boss>().Damage();
            Destroy(gameObject);

        }

        if (collision.gameObject.tag == "Obstacle")
        {
            return;
        }

        // Stop the object and play destroy effect
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(PlayExplosionParticles());

    }

    public override void SaveBullet()
    {
        Destroy(gameObject);
    }
}
