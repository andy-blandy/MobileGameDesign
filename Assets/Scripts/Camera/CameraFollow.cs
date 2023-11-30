using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // References
    private Transform player;
    private GameManager gameManager;

    [Header("Position")]
    public float xOffset;
    public float yOffset;
    public float zOffset;

    #region Singleton
    public static CameraFollow instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void LateUpdate()
    {
        player = gameManager.player;

        float newX = player.position.x + xOffset;
        float newY = player.position.y + yOffset;
        float newZ = player.position.z + zOffset;
        Vector3 newPos = new Vector3(newX, newY, newZ);

        transform.position = newPos;
    }

    public void SetOffsets(float newXOffset, float newYOffset, float newZOffset)
    {
        xOffset = newXOffset;
        yOffset = newYOffset;
        zOffset = newZOffset;
    }
}
