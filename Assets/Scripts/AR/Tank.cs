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

    [Header("Primary Fire")]
    public Transform primaryTurretSpawn;
    public GameObject primaryProjectile;
    public Stack<GameObject> primaryProjectileContainer = new Stack<GameObject>();

    [Header("Secondary Fire")]
    public List<Transform> aimingGrid;
    public GameObject secondaryProjectile;
    public Stack<GameObject> secondaryProjectileContainer = new Stack<GameObject>();
    public Transform rightTurret, leftTurret;
    public Transform rightTurretSpawn, leftTurretSpawn;

    [Header("Attacks")]
    public float timeBetweenAttacks;
    public float shotTimer;
    public int orderInAttacks;
    YieldInstruction waitBetweenShots = new WaitForSeconds(0.2f);

    [Header("Aiming")]
    public float headTurnSpeed;
    public float gunTurnSpeed;

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

        // Set logic
        orderInAttacks = 0;
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
        Quaternion headRotation = Quaternion.LookRotation(playerHorizPosition - headTransform.position);
        headRotation = Quaternion.Slerp(headTransform.rotation, headRotation, headTurnSpeed * Time.deltaTime);
        headTransform.rotation = headRotation;

        // Gun Rotation
        float hypMagnitude = (arCameraPosition - gunTransform.position).magnitude;
        float oppMagnitude = arCameraPosition.y - gunTransform.position.y;
        float gunAngle = Mathf.Asin(oppMagnitude / hypMagnitude);
        gunAngle *= -180f / Mathf.PI;
        gunAngle = Mathf.Clamp(gunAngle, -70, 15);
        gunAngle = Mathf.LerpAngle(gunTransform.localEulerAngles.x, gunAngle, gunTurnSpeed * Time.deltaTime);
        gunTransform.localEulerAngles = new Vector3(gunAngle, 0f, 0f);
    }

    void CountdownTimer()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > timeBetweenAttacks)
        {
            Attack();
            shotTimer = 0;
        }
    }

    void Attack()
    {
        switch (orderInAttacks)
        {
            case 0:
                StartCoroutine(EasyAttack1());
                orderInAttacks++;
                break;
            case 1:
                StartCoroutine(EasyAttack2());
                orderInAttacks++;
                break;
            case 2:
                StartCoroutine(EasyAttack3());
                orderInAttacks = 0;
                break;
        }
    }

    void PrimaryShoot()
    {
        if (primaryProjectileContainer.TryPop(out GameObject proj))
        {
            proj.SetActive(true);
            proj.transform.position = primaryTurretSpawn.position;
            proj.transform.rotation = primaryTurretSpawn.rotation;
        }
        else
        {
            Instantiate(primaryProjectile, primaryTurretSpawn.position, primaryTurretSpawn.rotation);
        }
    }

    void SecondaryShoot(int locationA, int locationB)
    {
        // 0 is top left, 24 is bottom right

        // Aim turrets
        rightTurret.LookAt(aimingGrid[locationA]);
        leftTurret.LookAt(aimingGrid[locationB]);

        // Fire projectiles
        if (secondaryProjectileContainer.TryPop(out GameObject projA))
        {
            projA.transform.position = rightTurretSpawn.position;
            projA.transform.rotation = rightTurretSpawn.rotation;
            projA.SetActive(true);
        } else
        {
            Instantiate(secondaryProjectile, rightTurretSpawn.position, rightTurretSpawn.rotation);
        }

        if (secondaryProjectileContainer.TryPop(out GameObject projB))
        {
            projB.transform.position = leftTurretSpawn.position;
            projB.transform.rotation = leftTurretSpawn.rotation;
            projB.SetActive(true);
        }
        else
        {
            Instantiate(secondaryProjectile, leftTurretSpawn.position, leftTurretSpawn.rotation);
        }
    }

    IEnumerator EasyAttack1()
    {

        SecondaryShoot(16, 18);
        yield return waitBetweenShots;
        SecondaryShoot(11, 13);
        yield return waitBetweenShots;
        SecondaryShoot(6, 8);
    }

    IEnumerator EasyAttack2()
    {
        SecondaryShoot(18, 6);
        yield return waitBetweenShots;
        SecondaryShoot(17, 7);
        yield return waitBetweenShots;
        SecondaryShoot(16, 8);
    }

    IEnumerator EasyAttack3()
    {
        SecondaryShoot(20, 4);
        yield return waitBetweenShots;
        SecondaryShoot(16, 8);
        yield return waitBetweenShots;
        PrimaryShoot();
    }
}
