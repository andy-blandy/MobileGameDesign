using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hitboxes")]
    public GameObject runningHitbox;
    public GameObject slidingHitbox;
    public GameObject attackHitBox;
    public Transform feet;

    [Header("Falling")]
    public bool isGrounded;

    [Header("Running")]
    public float runSpeed = 1.0f;
    public bool runEndlessly;

    [Header("Jump")]
    public bool jumpCooling;
    public float jumpXForce = 0f;
    public float jumpYForce = 1f;
    public float timeBetweenJumps = 0.5f;

    [Header("Slide")]
    public float slideRotation = 60f;
    public float slidePos = -0.1f;
    public GameObject bodyToRotate;
    private bool isSliding;

    [Header("Melee Attack")]
    public CanvasGroup meleeAttackVisual; // We use a canvasgroup to affect the transparency of the attack
    public float rotationAmount = 10.0f;
    public int animationSteps = 10;
    public float frameLength = 0.01f;
    private float startingRotationZ = 60f;
    private bool isAttacking;

    public bool isSwipingEnabled;

    public AudioClip[] soundEffects;
    AudioSource audioSource = null;

    private Rigidbody m_rigidbody;

    private GameManager gameManager;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Set hitboxes
        slidingHitbox.SetActive(false);
        runningHitbox.SetActive(true);
        attackHitBox.SetActive(false);

        // Set rotation
        Vector3 normRot = new Vector3(bodyToRotate.transform.eulerAngles.x, bodyToRotate.transform.eulerAngles.y, 0f);
        bodyToRotate.transform.eulerAngles = normRot;
        Vector3 normPos = new Vector3(bodyToRotate.transform.position.x, transform.position.y, bodyToRotate.transform.position.z);
        bodyToRotate.transform.position = normPos;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Allow player to jump when player is on the ground and presses space
        if (Input.GetKeyDown(KeyCode.W) && !jumpCooling && isGrounded)
        {
            Jump();
        }

        // Sliding
        // Bring player into slide
        if (Input.GetKey(KeyCode.S))
        {
            Slide();

        }

        // Bring player out of slide
        if (Input.GetKeyUp(KeyCode.S) || jumpCooling || !isGrounded)
        {
            ExitSlide();
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.D) && !isAttacking && !isSliding)
        {
            StartCoroutine(SlashAnimation());
        }

        // Run endlessly
        if (runEndlessly)
        {
            Vector3 move = new Vector3(1f, 0f, 0f);
            transform.Translate(move * runSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        // Add jump to rigidbody
        Vector3 forceOfJump = new Vector3(jumpXForce,
            jumpYForce,
            0f);
        m_rigidbody.AddForce(forceOfJump, ForceMode.Impulse);
        StartCoroutine(JumpCooldown());


        // Play sound effect
        audioSource.PlayOneShot(soundEffects[0], 5.0f);
    }

    IEnumerator JumpCooldown()
    {
        jumpCooling = true;

        yield return new WaitForSeconds(timeBetweenJumps);

        jumpCooling = false;
    }

    IEnumerator SlashAnimation()
    {
        isAttacking = true;

        // Make attack visual visible
        meleeAttackVisual.gameObject.SetActive(true);
        meleeAttackVisual.alpha = 1f;

        // Set attack visual to starting position
        Vector3 startingRotation = new Vector3(0f, 0f, startingRotationZ);
        meleeAttackVisual.transform.eulerAngles = startingRotation;

        // Activate hitbox
        attackHitBox.SetActive(true);

        // Animate
        Vector3 rot = new Vector3(0f, 0f, rotationAmount * -1);
        float transparencyChangeAmount = 1f / animationSteps;
        for (int i = 0; i < animationSteps; i++)
        {
            yield return new WaitForSeconds(frameLength);
            meleeAttackVisual.transform.Rotate(rot);
            meleeAttackVisual.alpha -= transparencyChangeAmount;
        }

        // Deactivate all the stuff
        attackHitBox.SetActive(false);
        meleeAttackVisual.gameObject.SetActive(false);
        isAttacking = false;
    }

    void Slide()
    {
        if (jumpCooling || !isGrounded)
        {
            return;
        }

        // Visuals
        Vector3 newRot = new Vector3(bodyToRotate.transform.eulerAngles.x, bodyToRotate.transform.eulerAngles.y, slideRotation);
        bodyToRotate.transform.eulerAngles = newRot;
        Vector3 newPos = new Vector3(bodyToRotate.transform.position.x, transform.position.y + slidePos, bodyToRotate.transform.position.z);
        bodyToRotate.transform.position = newPos;

        isSliding = true;


        // Set hitboxes
        slidingHitbox.SetActive(true);
        runningHitbox.SetActive(false);

        // Play audio
        audioSource.PlayOneShot(soundEffects[1], 0.7f);
    }

    void ExitSlide()
    {
        // Visuals
        Vector3 normRot = new Vector3(bodyToRotate.transform.eulerAngles.x, bodyToRotate.transform.eulerAngles.y, 0f);
        bodyToRotate.transform.eulerAngles = normRot;
        Vector3 normPos = new Vector3(bodyToRotate.transform.position.x, transform.position.y, bodyToRotate.transform.position.z);
        bodyToRotate.transform.position = normPos;

        isSliding = false;

        // Set hitboxes
        slidingHitbox.SetActive(false);
        runningHitbox.SetActive(true);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void FixedUpdate()
    {
        isGrounded = false;
    }

    #region Touch Controls
    public void ButtonJump()
    {
        if (!jumpCooling && isGrounded)
        {
            Jump();
        }
    }

    public void ButtonAttack()
    {
        if (!isAttacking && !isSliding)
        {
            StartCoroutine(SlashAnimation());
        }
    }

    public void ButtonSlide()
    {
        if (isSliding)
        {
            ExitSlide();
        }
        else
        {
            Slide();
        }
    }
    #endregion
}
