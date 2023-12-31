using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

public class AR_Player : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public float invincibilityTimer = 0.1f;
    public bool isInvincible;

    [Header("Animation")]
    public Animator handAnimator;

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Do nothing if the finger is over a UI object
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            Attack();
        }
    }

    public void Damage()
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth--;
        isInvincible = true;

        AR_HealthGUI.instance.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            AR_GameManager.instance.LoseGame();
        }

        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(invincibilityTimer);
        isInvincible = false;
    }

    public void Attack()
    {
        if (handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            return;
        }

        handAnimator.Play("Attack_1");

    }
}
