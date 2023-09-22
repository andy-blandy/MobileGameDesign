using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    public GameObject jumpButton;
    public GameObject slideButton;
    public GameObject attackButton;

    private Button m_jumpButton, m_slideButton, m_attackButton;
    private Sprite m_jumpSprite, m_slideSprite, m_attackSprite;
    private List<Button> m_allButtons = new List<Button>();
    private GameManager gameManager;

    public string currentControlState;

    [HideInInspector] public ControlsManager instance;

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
    }

    void Start()
    {
        gameManager = GameManager.instance;

        m_jumpButton.onClick.AddListener(ButtonJump);
        m_attackButton.onClick.AddListener(ButtonAttack);
        m_slideButton.onClick.AddListener(ButtonSlide);
    }

    void Update()
    {
        switch (currentControlState) 
        {
            case "buttons":
                break;
            case "tap":
                TapUpdate();
                break;
            case "swipe":
                break;
            default:
                EnableButtons();
                break;
        }
    }

    public void EnableButtons()
    {
        foreach (Button button in m_allButtons)
        {
            button.enabled = true;
            button.gameObject.SetActive(true);
        }
        currentControlState = "buttons";
    }

    void DisableButtons()
    {
        foreach (Button button in m_allButtons)
        {
            button.enabled = false;
            button.gameObject.SetActive(false);
        }
    }

    public void EnableSwiping()
    {
        currentControlState = "swipe";

        DisableButtons();
    }

    public void EnableTapping()
    {
        currentControlState = "tap";

        DisableButtons();
    }

    private void TapUpdate()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began ||
                touch.phase == TouchPhase.Ended)
            {
                ButtonSlide();
            }
        } else if (Input.touchCount == 2)
        {
            ButtonJump();
        }
    }

    private void ButtonJump()
    {
        gameManager.playerMovementScript.ButtonJump();
    }

    private void ButtonAttack()
    {
        gameManager.playerMovementScript.ButtonAttack();
    }

    private void ButtonSlide()
    {
        gameManager.playerMovementScript.ButtonSlide();
    }
}
