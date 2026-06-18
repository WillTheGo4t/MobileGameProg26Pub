using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARBaitSpawner : MonoBehaviour
{
    public enum SpawnMode { None, Fruit, Cage }
    public SpawnMode currentMode = SpawnMode.None;

    [SerializeField] private GameObject baitPrefab;
    [SerializeField] private GameObject cagePrefab;

    [SerializeField] private Image fruitButtonImage;
    [SerializeField] private Image cageButtonImage;

    [SerializeField] private ARRaycastManager raycastManager;
    private GameObject spawnedCage;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private PlayerInputSystem _playerInput;

    public static event Action<Transform> OnBaitPlaced;

    void Awake()
    {
        _playerInput = new PlayerInputSystem();
    }

    void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Touch.PrimaryTouch.performed += OnPrimaryTouch;
    }

    void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Touch.PrimaryTouch.performed -= OnPrimaryTouch;
    }

    public void ToggleModeFruit()
    {
        currentMode = (currentMode == SpawnMode.Fruit) ? SpawnMode.None : SpawnMode.Fruit;
        UpdateUI();
    }

    public void ToggleModeCage()
    {
        currentMode = (currentMode == SpawnMode.Cage) ? SpawnMode.None : SpawnMode.Cage;
        UpdateUI();
    }

    public void Flee()
    {
        SceneManager.LoadScene("MapScene");
    }

    void UpdateUI()
    {
        if (fruitButtonImage != null) fruitButtonImage.color = (currentMode == SpawnMode.Fruit) ? Color.green : Color.white;
        if (cageButtonImage != null) cageButtonImage.color = (currentMode == SpawnMode.Cage) ? Color.green : Color.white;
    }

    void TouchHandling()
    {
        if (currentMode == SpawnMode.None) return;

        Vector2 touchPosition = _playerInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>();
        bool hasHitSomething = raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);

        if (hasHitSomething)
        {
            Pose hitPose = hits[0].pose;

            if (currentMode == SpawnMode.Fruit)
            {
                if (GameplayManager.Instance.TryUseFruit())
                {
                    GameObject newBait = Instantiate(baitPrefab, hitPose.position, hitPose.rotation);
                    OnBaitPlaced?.Invoke(newBait.transform);
                }
            }
            else if (currentMode == SpawnMode.Cage)
            {
                ARCreature creature = FindAnyObjectByType<ARCreature>();
                if (creature != null && Vector3.Distance(hitPose.position, creature.transform.position) < creature.minCageDistance)
                {
                    return;
                }

                if (spawnedCage != null) Destroy(spawnedCage);
                spawnedCage = Instantiate(cagePrefab, hitPose.position, hitPose.rotation);
            }
        }
    }

    void OnPrimaryTouch(InputAction.CallbackContext context)
    {
        TouchHandling();
    }
}