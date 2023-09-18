using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 2.0f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        AddMovement();
    }

    public void AddMovement()
    {
        rb.AddForce(transform.right * bulletSpeed, ForceMode.Impulse);
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}
