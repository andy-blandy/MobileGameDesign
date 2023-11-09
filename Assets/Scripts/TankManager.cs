using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class TankManager : MonoBehaviour
{
    public Transform arCameraTransform;
    public GameObject tankGameObject;
    public Tank tankScript;

    [Header("Touch")]
    public float touchXPosition;

    public static TankManager instance;
    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        if (tankGameObject == null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                return;
            }

            Touch touch = Input.GetTouch(0);

            // Rotation around the y-axis
            touchXPosition = touch.position.x;
            float rotationAmount = 0f;

            if (touchXPosition == Screen.width / 2)
            {
                return;
            }

            if (touchXPosition < Screen.width / 4)
            {
                rotationAmount = 1f;
            } else if (touchXPosition < Screen.width / 2)
            {
                rotationAmount = 0.5f;
            } else if (touchXPosition < ((Screen.width / 2) + (Screen.width / 4)))
            {
                rotationAmount = -0.5f;
            } else
            {
                rotationAmount = -1f;
            }

            Vector3 rotationVector = new Vector3(0f, rotationAmount * 5, 0f);
            tankGameObject.transform.Rotate(rotationVector);
            
        }
    }

    public void AnimateTank()
    {
        if (tankGameObject == null)
        {
            return;
        }

        tankScript.animator.Play("TankSpawn");
    }

    public void SwitchScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
