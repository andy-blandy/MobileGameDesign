using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Animator animator;

    [Header("GameObjects")]
    public GameObject tankGameObject;
    public Transform headTransform;
    public Transform gunTransform;

    [Header("Projectiles")]
    public Transform primaryTurretSpawn;
    public GameObject primaryProjectile;
    public Stack<GameObject> primaryProjectileContainer = new Stack<GameObject>();
    public float timeBetweenShots;
    public float shotTimer;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        TankManager.instance.tankGameObject = tankGameObject;
        TankManager.instance.tankScript = this;

        // Play the spawn animation
        animator.Play("TankSpawn");
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TankSpawn"))
        {
            return;
        }

        LookTowardsPlayer();

        CountdownTimer();
    }

    void LookTowardsPlayer()
    {
        Vector3 arCameraPosition = TankManager.instance.arCameraTransform.position;

        // Head Rotation
        Vector3 playerHorizPosition = new Vector3(arCameraPosition.x, headTransform.position.y, arCameraPosition.z);
        headTransform.LookAt(playerHorizPosition);

        // Gun Rotation
        float hypMagnitude = (arCameraPosition - gunTransform.position).magnitude;
        float oppMagnitude = arCameraPosition.y - gunTransform.position.y;
        float gunAngle = Mathf.Asin(oppMagnitude / hypMagnitude);
        gunAngle *= -180f / Mathf.PI;
        gunAngle = Mathf.Clamp(gunAngle, -70, 15);
        gunTransform.localEulerAngles = new Vector3(gunAngle, 0f, 0f);

        // Debug
        string debugText = "Hyp: " + hypMagnitude.ToString() + ", Opp: " + oppMagnitude.ToString() + ", Gun Angle: " + gunAngle.ToString();
        TankManager.instance.SetDebugText(debugText);
    }

    void CountdownTimer()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > timeBetweenShots)
        {
            PrimaryShoot();
            shotTimer = 0;
        }
    }

    void PrimaryShoot()
    {
        if (primaryProjectileContainer.TryPop(out GameObject proj))
        {
            proj.transform.position = primaryTurretSpawn.position;
            proj.transform.rotation = primaryTurretSpawn.rotation;
            return;
        }
    }
}
