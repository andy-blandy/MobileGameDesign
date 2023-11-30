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

    [Header("Death")]
    public bool isDead;

    [Header("Animation")]
    public bool switchingAnimation;
    public float animationCooldown = 0.1f;

    [Header("Controls")]
    public bool isSwipingEnabled;

    [Header("SFX")]
    public AudioClip[] soundEffects;
    AudioSource audioSource = null;

    [Header("Components")]
    public Animator animator;
    private Rigidbody m_rigidbody;

    private GameManager gameManager;
    public static PlayerMovement instance;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        instance = this;
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
        if (isDead)
        {
            return;
        }

        // Allow player to jump when player is on the ground and presses space
        if (Input.GetKeyDown(KeyCode.W) && !jumpCooling && isGrounded)
        {
            Jump();
        }

        // Sliding
        // Bring player into slide
        if (Input.GetKey(KeyCode.S))
        {
            EnterSlide();

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

        if (!isGrounded && !isAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping Anim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Anim"))
        {
            animator.Play("Falling Anim");
        }

        if (isGrounded && !isSliding && !isAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName("Running Animation") && !switchingAnimation)
        {
            animator.CrossFade("Running Animation", 0.1f);
            StartCoroutine(AnimationSwitchCooldown());
        }
    }

    IEnumerator AnimationSwitchCooldown()
    {
        switchingAnimation = true;

        yield return new WaitForSeconds(animationCooldown);

        switchingAnimation = false;
    }

    public void Jump()
    {
        if (jumpCooling || !isGrounded)
        {
            return;
        }

        // Animate
        animator.CrossFade("Jumping Anim", 1f);

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

        // Animate
        animator.CrossFade("Attack Anim", 0.3f);

        yield return new WaitForSeconds(0.5f);

        // Deactivate all the stuff
        meleeAttackVisual.gameObject.SetActive(false);
        isAttacking = false;
    }

    public void EnterSlide()
    {
        if (!isGrounded || isSliding)
        {
            return;
        }

        isSliding = true;

        // Animation
        animator.CrossFade("Slide", 0.2f);

        // Set hitboxes
        slidingHitbox.SetActive(true);
        runningHitbox.SetActive(false);

        // Play audio
        audioSource.PlayOneShot(soundEffects[1], 0.7f);
    }

    public void ExitSlide()
    {
        if (!isSliding)
        {
            return;
        }

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
    #endregion

    public IEnumerator PlayerDeath()
    {
        animator.CrossFade("Dying Anim", 0.2f);
        isDead = true;

        yield return new WaitForSeconds(1f);

        GameManager.instance.PlayerIsHit();
        isDead = false;
    }
}
