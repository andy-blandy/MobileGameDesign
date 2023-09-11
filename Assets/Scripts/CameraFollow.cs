using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float xOffset;
    public float yOffset;

    public SpawnLevel spawnLevel;

    #region Singleton
    public static CameraFollow instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        spawnLevel = SpawnLevel.instance;
    }

    void LateUpdate()
    {
        player = spawnLevel.player;

        float newX = player.position.x + xOffset;
        float newY = player.position.y + yOffset;
        Vector3 newPos = new Vector3(newX, newY, transform.position.z);

        transform.position = newPos;
    }
}
