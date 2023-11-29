using System;
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
    [SerializeField] ARPlaneManager m_PlaneManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    const TrackableType trackableTypes = TrackableType.PlaneWithinPolygon;

    [Header("Aiming")]
    public List<Transform> aimingGrid;

    public static TankManager instance;
    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        m_PlaneManager.planesChanged += FindPointOnPlane;
    }

    private void FindPointOnPlane(ARPlanesChangedEventArgs args)
    {
        if (m_RaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), m_Hits, trackableTypes))
        {
            SpawnTank(m_Hits[0].pose.position);
            m_PlaneManager.planesChanged -= FindPointOnPlane;
        }
    }

    public void SpawnTank(Vector3 spawnPosition)
    {
        tankGameObject = Instantiate(tankPrefab, spawnPosition, Quaternion.identity);
        tankScript = tankGameObject.GetComponent<Tank>();
    }

    public void AnimateTank()
    {
        if (tankGameObject == null)
        {
            return;
        }

        tankScript.animator.Play("TankSpawn");
    }
}
