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
        Vector3 newPos = new Vector3(newX, newY, transform.position.z);

        transform.position = newPos;
    }
}
