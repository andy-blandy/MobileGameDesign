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
    public float bodyTurnSpeed;

    [Header("Stats")]
    public int health;

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
        health = 3;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TankSpawn"))
        {
            return;
        }

        MoveTowardsPlayer();
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

    void MoveTowardsPlayer()
    {
        Vector3 arCameraPosition = TankManager.instance.arCameraTransform.position;

        // Body Rotation
        Vector3 playerHorizPosition = new Vector3(arCameraPosition.x, transform.position.y, arCameraPosition.z);
        Vector3 deltaPosition = playerHorizPosition - transform.position;

        if (deltaPosition != Vector3.zero)
        {
            Quaternion bodyRotation = Quaternion.LookRotation(deltaPosition);
            bodyRotation = Quaternion.Slerp(transform.rotation, bodyRotation, bodyTurnSpeed * Time.deltaTime);
            transform.rotation = bodyRotation;
        }
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
                Attack1();
                orderInAttacks++;
                break;
            case 1:
                Attack2();
                orderInAttacks++;
                break;
            case 2:
                Attack3();
                orderInAttacks = 0;
                break;
        }
    }

    void PrimaryShoot()
    {
        if (primaryProjectileContainer.TryPop(out GameObject proj))
        {
            proj.transform.position = primaryTurretSpawn.position;
            proj.transform.rotation = primaryTurretSpawn.rotation;
            proj.SetActive(true);
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
        rightTurret.LookAt(TankManager.instance.aimingGrid[locationA]);
        leftTurret.LookAt(TankManager.instance.aimingGrid[locationB]);

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

    public void Damage()
    {
        health--;

        if (health <= 0)
        {
            DestroyTank();
        }
    }

    public void DestroyTank()
    {
        AR_GameManager.instance.WinGame();
    }

    void Attack1()
    {
        switch (health)
        {
            case 3:
                StartCoroutine(EasyAttack1());
                break;
            case 2:
                StartCoroutine(MedAttack1());
                break;
            case 1:
                StartCoroutine(HardAttack1());
                break;
        }
    }

    void Attack2()
    {
        switch (health)
        {
            case 3:
                StartCoroutine(EasyAttack2());
                break;
            case 2:
                StartCoroutine(MedAttack2());
                break;
            case 1:
                StartCoroutine(HardAttack2());
                break;
        }
    }

    void Attack3()
    {
        switch (health)
        {
            case 3:
                StartCoroutine(EasyAttack3());
                break;
            case 2:
                StartCoroutine(MedAttack3());
                break;
            case 1:
                StartCoroutine(HardAttack3());
                break;
        }
    }

    IEnumerator EasyAttack1()
    {
        SecondaryShoot(18, 16);
        yield return waitBetweenShots;
        yield return waitBetweenShots;
        SecondaryShoot(8, 6);
    }

    IEnumerator EasyAttack2()
    {
        SecondaryShoot(6, 18);
        yield return waitBetweenShots;
        yield return waitBetweenShots;
        SecondaryShoot(8, 16);
    }

    IEnumerator EasyAttack3()
    {
        SecondaryShoot(4, 20);
        yield return waitBetweenShots;
        yield return waitBetweenShots;
        PrimaryShoot();
    }

    IEnumerator MedAttack1()
    {
        SecondaryShoot(4, 0);
        yield return waitBetweenShots;
        SecondaryShoot(9, 5);
        yield return waitBetweenShots;
        yield return waitBetweenShots;
        SecondaryShoot(14, 10);
    }

    IEnumerator MedAttack2()
    {
        SecondaryShoot(2, 1);
        yield return waitBetweenShots;
        SecondaryShoot(8, 7);
        yield return waitBetweenShots;
        yield return waitBetweenShots;
        SecondaryShoot(14, 13);
    }

    IEnumerator MedAttack3()
    {
        SecondaryShoot(13, 11);
        yield return waitBetweenShots;
        SecondaryShoot(14, 10);
        PrimaryShoot();
    }

    IEnumerator HardAttack1()
    {
        SecondaryShoot(8, 6);
        yield return waitBetweenShots;
        SecondaryShoot(13, 11);
        yield return waitBetweenShots;
        SecondaryShoot(7, 7);
    }

    IEnumerator HardAttack2()
    {
        SecondaryShoot(4, 20);
        yield return waitBetweenShots;
        SecondaryShoot(8, 18);
        yield return waitBetweenShots;
        SecondaryShoot(12, 12);
        yield return waitBetweenShots;
        SecondaryShoot(18, 6);
        yield return waitBetweenShots;
        SecondaryShoot(24, 0);
    }

    IEnumerator HardAttack3()
    {
        SecondaryShoot(11, 13);
        yield return waitBetweenShots;
        SecondaryShoot(16, 8);
        yield return waitBetweenShots;
        SecondaryShoot(7, 17);
        PrimaryShoot();
    }
}
