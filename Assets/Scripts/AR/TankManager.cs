using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TankManager : MonoBehaviour
{
    public Transform arCameraTransform;
    public GameObject tankGameObject;
    public Tank tankScript;

    public GameObject tankPrefab;

    [Header("AR")]
    [SerializeField] ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    const TrackableType trackableTypes = TrackableType.PlaneWithinPolygon;

    public static TankManager instance;
    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        if (tankGameObject != null)
        {
            return;
        }

        if (m_RaycastManager.Raycast(arCameraTransform.position, m_Hits, trackableTypes))
        {
            SpawnTank(m_Hits[0].pose.position);
        }
    }

    public void SpawnTank(Vector3 spawnPosition)
    {
        tankGameObject = Instantiate(tankPrefab, spawnPosition, Quaternion.identity);
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
