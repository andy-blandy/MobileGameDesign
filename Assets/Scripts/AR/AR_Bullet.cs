using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Bullet : Bullet
{
    public bool deflected;

    void Start()
    {
        deflected = false;
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "AR_Enemy")
        {
            TankManager.instance.tankScript.Damage();
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

        Vector3 tankPos = TankManager.instance.tankGameObject.transform.position;

        transform.LookAt(tankPos);
        rb.AddForce(transform.forward * bulletSpeed * 2, ForceMode.Impulse);
    }
}
