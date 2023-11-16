using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Bullet : Bullet
{
    public override void SaveBullet()
    {
        TankManager.instance.tankScript.primaryProjectileContainer.Push(gameObject);
    }
}
