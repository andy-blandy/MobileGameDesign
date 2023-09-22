using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    private GameManager m_gameManager;


    void Start()
    {
        m_gameManager = GameManager.instance;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

        }
    }
}
