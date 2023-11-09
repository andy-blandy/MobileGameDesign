using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject tankGameObject;
    public Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        TankManager.instance.tankGameObject = tankGameObject;
        TankManager.instance.tankScript = this;

        // Have the tank face player
        LookTowardsPlayer();

        // Play the spawn animation
        animator.Play("TankSpawn");
    }

    void LookTowardsPlayer()
    {
        Vector3 arCameraPosition = TankManager.instance.arCameraTransform.position;
        Vector3 playerPosition = new Vector3(arCameraPosition.x, tankGameObject.transform.position.y, arCameraPosition.z);
        tankGameObject.transform.LookAt(playerPosition);
    }
}
