using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, InputActions.IGameplayActions
{
    [Header("Hitboxes")]
    public GameObject runningHitbox;
    public GameObject slidingHitbox;
    public GameObject attackHitBox;
    public Transform feet;

    [Header("Falling")]
    public float distToGround = 0.01f;
    public bool isGrounded;
    public float gravity = 9.8f;

    [Header("Running")]
    public float runSpeed = 1.0f;
    public bool runEndlessly;

    [Header("Jump")]
    public bool isJumping;
    public float jumpHeight;
    public float jumpSpeed;
    public int numOfSteps;

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

    private GameManager gameManager;

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
        // Use a raycast to see if player is touching ground or not
        isGrounded = Physics.BoxCast(feet.position, feet.localScale / 2, Vector3.down, Quaternion.identity, distToGround);


        // Allow player to jump when player is on the ground and presses space
        if (Input.GetKeyDown(KeyCode.W) && !isJumping && isGrounded)
        {
            StartCoroutine("Jump");
            audioSource.PlayOneShot(soundEffects[0], 5.0f);
        }

        // Sliding
        // Bring player into slide
        if (Input.GetKey(KeyCode.S))
        {
            Slide();
            audioSource.PlayOneShot(soundEffects[1], 0.7f);
        }

        // Bring player out of slide
        if (Input.GetKeyUp(KeyCode.S) || isJumping || !isGrounded)
        {
            ExitSlide();
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.D) && !isAttacking && !isSliding)
        {
            StartCoroutine(SlashAnimation());
        }
    }

    void LateUpdate()
    {
        if (!isGrounded && !isJumping)
        {
            // Apply gravity
            transform.Translate(Vector3.down * gravity * Time.deltaTime);
        }

        // Run endlessly
        if (runEndlessly) {
            Vector3 move = new Vector3(1f, 0f, 0f);
            transform.Translate(move * runSpeed * Time.deltaTime);
        }
    }

    public void ButtonJump()
    {
        if (!isJumping && isGrounded)
        {
            StartCoroutine(Jump());
            audioSource.PlayOneShot(soundEffects[0], 5.0f);
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
        } else
        {
            Slide();
            audioSource.PlayOneShot(soundEffects[1], 0.7f);
        }
    }

    void Slide()
    {
        if (isJumping || !isGrounded)
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

    IEnumerator Jump()
    {
        isJumping = true;

        float yCounter = 0;
        float jumpAmount = jumpHeight / numOfSteps;

        while (yCounter < jumpHeight)
        {
            Vector3 movement = Vector3.up * jumpAmount;
            yCounter += jumpAmount;

            transform.Translate(movement);
            yield return new WaitForSeconds(1 / jumpSpeed);
        }

        isJumping = false;
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

    public void OnSwipe(InputAction.CallbackContext callback)
    {
        if (!isSwipingEnabled)
        {
            return;
        }

        float inputMovement = callback.ReadValue<float>();
        Debug.Log(inputMovement);

        if (inputMovement > 0f)
        {
            ButtonJump();
        }
        else if (inputMovement < 0f)
        {
            ButtonSlide();
        }
    }

    public void OnAnalog(InputAction.CallbackContext callback)
    {
        Vector2 inputMovement = callback.ReadValue<Vector2>();

        if (inputMovement.y > 0f)
        {
            ButtonJump();
        }
        else if (inputMovement.y < 0f)
        {
            ButtonSlide();
        }
    }

    public void ChangePlayerSpeed(float speedAmount)
    {
        runSpeed = speedAmount;
    }
}
