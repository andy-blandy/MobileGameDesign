using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBoss : Boss
{
    [Header("Prefabs")]
    public GameObject regularBullet;
    public GameObject deflectableBullet;

    [Header("Spawn Positions")]
    public Transform bulletSpawn;
    public Transform deflectableBulletSpawn;

    [Header("Attack")]
    public float timeBetweenEachShot;
    public float numberOfRegularShots;

    [Header("Settings")]
    public float distanceFromPlayer = 15f;

    private Transform playerInstance;

    void Awake()
    {
        numberOfPhases = 2;
        currentPhase = 0;
    }

    void Start()
    {
        playerInstance = GameManager.instance.player;
    }

    void Update()
    {
        Vector3 newPosition = new Vector3(playerInstance.position.x + distanceFromPlayer,
            -19f,
            playerInstance.position.z);

        transform.position = newPosition;
    }

    public override void NextPhase()
    {
        switch (currentPhase)
        {
            case 0:
                // Nothing happens
                break;
            case 1:
                // Tank shoots
                StartCoroutine(ShootBullets());
                break;
            default:
                break;
        }

        currentPhase++;
        if (currentPhase == numberOfPhases)
        {
            currentPhase = 0;
        }
    }

    IEnumerator ShootBullets()
    {
        for (int i = 0; i < numberOfRegularShots; i++)
        {
            ShootRegularBullet();
            yield return new WaitForSeconds(timeBetweenEachShot);
        }

        yield return new WaitForSeconds(timeBetweenEachShot);
        ShootDeflectableBullet();

    }

    void ShootRegularBullet()
    {
        Instantiate(regularBullet, bulletSpawn.position, bulletSpawn.rotation);
    }

    void ShootDeflectableBullet() 
    {
        Instantiate(deflectableBullet, deflectableBulletSpawn.position, deflectableBulletSpawn.rotation);
    }
}
