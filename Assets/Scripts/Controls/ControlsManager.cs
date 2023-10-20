using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    #region Buttons
    public GameObject jumpButton, slideButton, attackButton;
    private Button m_jumpButton, m_slideButton, m_attackButton;
    private Sprite m_jumpSprite, m_slideSprite, m_attackSprite;
    private List<Button> m_allButtons = new List<Button>();
    #endregion

    public float tapCooldownTime;
    public float swipeCooldownTime;

    public bool isTapping;
    public bool isSwiping;

    [Range(0f, 100f)] public float swipeControlSensitivity = 1f;
    private bool isFingerDown;
    private Vector2 startSwipePosition;
    private Vector2 currentSwipePosition;
    private Coroutine swipeCooldownCoroutine;

    private GameManager gameManager;

    // The control state is stored in PlayerPrefs under the Key "ControlScheme"
    public string currentControlState;
    private Touch[] m_touches;

    [HideInInspector] public ControlsManager instance;

    [Header("Debug")]
    public TextMeshProUGUI touchPositionValueText;
    public TextMeshProUGUI controlStateText;
    public TextMeshProUGUI touchNumberText;
    public TextMeshProUGUI isSwipingDebugText;
    public TextMeshProUGUI isFingerDownDebugText;
    public GameObject startSwipePosGameObject;
    public TextMeshProUGUI startSwipePosDebugText;

    void Awake()
    {
        // Singleton
        instance = this;

        m_jumpButton = jumpButton.GetComponent<Button>();
        m_attackButton = attackButton.GetComponent<Button>();
        m_slideButton = slideButton.GetComponent<Button>();
        m_allButtons.Add(m_jumpButton);
        m_allButtons.Add(m_attackButton);
        m_allButtons.Add(m_slideButton);

        m_jumpSprite = jumpButton.GetComponent<Sprite>();
        m_attackSprite = attackButton.GetComponent<Sprite>();
        m_slideSprite = slideButton.GetComponent<Sprite>();

        #region Variables
        isFingerDown = false;
        isSwiping = false;
        #endregion
    }

    void Start()
    {
        gameManager = GameManager.instance;

        #region Button Controls
        // JUMPING
        EventTrigger jumpTrigger = jumpButton.GetComponent<EventTrigger>();
        EventTrigger.Entry jumpEntry = new EventTrigger.Entry();
        jumpEntry.eventID = EventTriggerType.PointerDown;
        jumpEntry.callback.AddListener((data) => { ButtonJump(); });
        jumpTrigger.triggers.Add(jumpEntry);

        // ATTACKING
        EventTrigger attackTrigger = attackButton.GetComponent <EventTrigger>();
        EventTrigger.Entry attackEntry = new EventTrigger.Entry();
        attackEntry.eventID = EventTriggerType.PointerDown;
        attackEntry.callback.AddListener((data) => { ButtonAttack(); });
        attackTrigger.triggers.Add(attackEntry);

        // SLIDING
        EventTrigger slideTrigger = slideButton.GetComponent<EventTrigger>();
        EventTrigger.Entry slideEnterEntry = new EventTrigger.Entry();
        slideEnterEntry.eventID = EventTriggerType.PointerDown;
        slideEnterEntry.callback.AddListener((data) => { ButtonEnterSlide(); });
        EventTrigger.Entry slideExitEntry = new EventTrigger.Entry();
        slideExitEntry.eventID = EventTriggerType.PointerUp;
        slideExitEntry.callback.AddListener((data) => { ButtonExitSlide(); });
        slideTrigger.triggers.Add(slideEnterEntry);
        slideTrigger.triggers.Add(slideExitEntry);
        #endregion

        // Set control state to the player's choice in PlayerPrefs
        if (currentControlState != PlayerPrefs.GetString(gameManager.controlStateKey))
        {
            SetControlsToPlayerPrefs();
        }
    }

    void Update()
    {
        #region Debugging
        touchNumberText.text = Input.touchCount.ToString();
        isSwipingDebugText.text = isSwiping.ToString();
        isFingerDownDebugText.text = isFingerDown.ToString();

        if (Input.touchCount > 0)
        {
            touchPositionValueText.text = Input.touches[0].position.ToString();
        }
        #endregion

        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }

        switch (currentControlState) 
        {
            case "buttons":
                ButtonsUpdate();
                break;
            case "tap":
                if (!isTapping)
                {
                    TapUpdate();
                }
                break;
            case "swipe":
                SwipeUpdate();
                break;
            default:
                SetControlsToButtons();
                break;
        }
    }

    #region Change Control State
    private void SetControlsToPlayerPrefs()
    {
        switch(PlayerPrefs.GetString(gameManager.controlStateKey))
        {
            case "buttons":
                SetControlsToButtons();
                break;
            case "swipe":
                SetControlsToSwiping();
                break;
            case "tap":
                SetControlsToTapping();
                break;
            default:
                SetControlsToButtons();
                break;
        }
    }

    public void SetControlsToButtons()
    {
        PlayerPrefs.SetString(gameManager.controlStateKey, "buttons");
        currentControlState = "buttons";
        EnableButtons();

        // Debug
        UpdateDebugger();
    }

    public void SetControlsToTapping()
    {
        PlayerPrefs.SetString(gameManager.controlStateKey, "swipe");
        currentControlState = "tap";
        DisableButtons();

        // Debug
        UpdateDebugger();
    }

    public void SetControlsToSwiping()
    {
        PlayerPrefs.SetString(gameManager.controlStateKey, "tap");
        currentControlState = "swipe";
        DisableButtons();

        // Debug
        UpdateDebugger();
    }

    private void UpdateDebugger()
    {
        controlStateText.text = currentControlState;

        if (currentControlState == "swipe")
        {
            startSwipePosGameObject.SetActive(true);
        } else
        {
            startSwipePosGameObject.SetActive(false);
        }
    }
    #endregion

    private void ButtonsUpdate()
    {

    }

    private void TapUpdate()
    {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);

            // If touching the right side of the screen...
            if (touch.position.x > (Screen.width / 2))
            {
                ButtonAttack();
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    ButtonEnterSlide();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    ButtonExitSlide();
                }
            }

            if (Input.touchCount == 2 && Input.GetTouch(1).position.x < (Screen.width / 2))
            {
                ButtonJump();
            }
        }

        isTapping = true;
        StartCoroutine(TapCooldown());
    }

    IEnumerator TapCooldown()
    {
        yield return new WaitForSeconds(tapCooldownTime);
        isTapping = false;
    }

    private void SwipeUpdate()
    {
        // If a finger has just touched the screen...
        if (Input.touchCount > 0 &&
            !isFingerDown &&
            Input.touches[0].phase == TouchPhase.Began)
        {
            startSwipePosition = Input.touches[0].position;
            isFingerDown = true;

            // Debug
            startSwipePosDebugText.text = startSwipePosition.ToString();
        }

        // If the finger is moving on the screen...
        if (isFingerDown &&
            !isSwiping &&
            Input.touches[0].phase == TouchPhase.Moved)
        {
            Vector2 endSwipePosition = Input.touches[0].position;

            // If the swipe began on the left side of the screen...
            if (startSwipePosition.x < (Screen.width / 2))
            {
                // Check for vertical direction of the swipe
                if (endSwipePosition.y > (startSwipePosition.y + swipeControlSensitivity)) // Swipe up
                {
                    gameManager.playerMovementScript.Jump();

                    startSwipePosition = Input.touches[0].position;

                    // Begin cooldown between swipes
                    isSwiping = true;
                    swipeCooldownCoroutine = StartCoroutine(SwipeCooldown());
                }
                else if (endSwipePosition.y < (startSwipePosition.y - swipeControlSensitivity)) // Swipe down
                {
                    gameManager.playerMovementScript.EnterSlide();

                    startSwipePosition = Input.touches[0].position;

                    // Begin cooldown between swipes
                    isSwiping = true;
                    swipeCooldownCoroutine = StartCoroutine(SwipeCooldown());
                }
            } else
            {
                // If the swipe began on the right side of the screen...
                if (endSwipePosition.y > (startSwipePosition.y + swipeControlSensitivity) ||
                    endSwipePosition.y < (startSwipePosition.y - swipeControlSensitivity))
                {
                    ButtonAttack();
                }
            }

        }

        // If the finger has left the screen...
        if (isFingerDown &&
            (Input.touchCount <= 0 ||
            Input.touches[0].phase == TouchPhase.Ended))
        {
            // Stop sliding
            gameManager.playerMovementScript.ExitSlide();

            // End swiping cooldown
            StopCoroutine(swipeCooldownCoroutine);
            isSwiping = false;

            // Update finger down bool
            isFingerDown = false;
        }


    }

    IEnumerator SwipeCooldown()
    {
        yield return new WaitForSeconds(swipeCooldownTime);
        isSwiping = false;
    }

    public void EnableButtons()
    {
        foreach (Button button in m_allButtons)
        {
            button.enabled = true;
            button.gameObject.SetActive(true);
        }
    }

    void DisableButtons()
    {
        foreach (Button button in m_allButtons)
        {
            button.enabled = false;
            button.gameObject.SetActive(false);
        }
    }

    public void ButtonJump()
    {
        gameManager.playerMovementScript.ButtonJump();
    }

    public void ButtonAttack()
    {
        gameManager.playerMovementScript.ButtonAttack();
    }

    public void ButtonEnterSlide()
    {
        gameManager.playerMovementScript.EnterSlide();
    }

    public void ButtonExitSlide()
    {
        gameManager.playerMovementScript.ExitSlide();
    }
}
