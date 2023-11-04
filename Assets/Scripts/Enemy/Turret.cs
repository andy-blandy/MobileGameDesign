using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Settings")]
    public float timeBetweenShots = 5f;
    private float timer;
    public float bulletLifespan = 3f;
    public float range = 20f;

    public GameObject bullet;
    public Transform bulletSpawn;

    private GameManager gameManager;

    [Header("Game Logic")]
    public float distanceFromPlayer;

    void Start()
    {
        gameManager = GameManager.instance;
        timer = timeBetweenShots - 1;
    }

    void Update()
    {
        distanceFromPlayer = transform.position.x - gameManager.player.transform.position.x;

        if (distanceFromPlayer < range)
        {
            Countdown();
        }
    }

    void Countdown()
    {
        if (timer < timeBetweenShots)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        GameObject b = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        timer = 0f;

        Destroy(b, bulletLifespan);
    }
}
