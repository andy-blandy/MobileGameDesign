using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_PlasmaBullet : Bullet
{
    public override void SaveBullet()
    {
        TankManager.instance.tankScript.secondaryProjectileContainer.Push(gameObject);
    }
}
