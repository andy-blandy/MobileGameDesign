using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float moveSpeed = -1f;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x += moveSpeed * Time.deltaTime;

        transform.position = newPos;
    }
}
