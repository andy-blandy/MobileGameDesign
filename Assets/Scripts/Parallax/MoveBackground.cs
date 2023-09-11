using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [Range(0f, 1f)] public float parallaxEffect = 1f;
    private CameraFollow cameraFollow;

    public List<GameObject> backgrounds;

    void Awake()
    {

    }

    void Start()
    {
        cameraFollow = CameraFollow.instance;
    }

    void Update()
    {
        // Set the position of the background to the position of the camera multiplied by the parallax effect
        Vector3 newPos = new Vector3(cameraFollow.transform.position.x * parallaxEffect, 
            cameraFollow.transform.position.y * parallaxEffect, 
            transform.position.z);
        transform.position = newPos;

        // If any of the backgrounds are off-screen, spawn them ahead of the camera
        foreach (GameObject background in backgrounds)
        {
            float length = background.GetComponent<SpriteRenderer>().bounds.size.x;

            if (background.transform.position.x <= cameraFollow.transform.position.x - length)
            {
                Vector3 newBackgroundPos = background.transform.position;
                newBackgroundPos.x = cameraFollow.transform.position.x + length;
                background.transform.position = newBackgroundPos;
            }
        }
    }
}
