using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Bullet : Bullet
{
    public bool deflected;
    public bool damagedTank;
    public AudioSource damageSound;

    public override void OnEnable()
    {
        deflected = false;
        damagedTank = false;

        base.OnEnable();
    }

    void Start()
    {
        damageSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!deflected)
        {
            return;
        }

        Vector3 tankPos = TankManager.instance.tankGameObject.transform.position;

        transform.LookAt(tankPos);
        rb.AddForce(transform.forward * 0.3f, ForceMode.Acceleration);
    }

    public override void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "AR_Tank" && !damagedTank)
        {
            Debug.Log("Hit TANK!");
            TankManager.instance.tankScript.Damage();
            damageSound.Play();
            damagedTank = true;

            Instantiate(AR_GameManager.instance.smokeEffect, other.contacts[0].point, Quaternion.identity, other.transform);
        }

        base.OnCollisionEnter(other);
    }

    public override void SaveBullet()
    {
        TankManager.instance.tankScript.primaryProjectileContainer.Push(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AR_Attack")
        {
            DeflectTowardsEnemy();
        }
    }

    void DeflectTowardsEnemy()
    {
        lifeTimer = 0;
        deflected = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }


}
