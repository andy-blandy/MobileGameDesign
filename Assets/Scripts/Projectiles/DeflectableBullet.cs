using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectableBullet : MonoBehaviour
{
    public float bulletSpeed = 2.0f;
    public float deflectSpeed = 10.0f;
    public bool isDeflected;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isDeflected = false;
    }

    void Start()
    {
        AddMovement();
    }

    public void AddMovement()
    {
        rb.AddForce(transform.right *  bulletSpeed, ForceMode.Impulse);
    }

    public void Deflect()
    {
        Vector3 rotationAmonut = new Vector3(0f, 180f, 0f);
        transform.Rotate(rotationAmonut);
        rb.AddForce(transform.right * deflectSpeed, ForceMode.Impulse);
        isDeflected = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && isDeflected)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
