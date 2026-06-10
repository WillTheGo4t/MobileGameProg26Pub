using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARBaitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject baitPrefab; // Das 3D-Modell deines Köders/deiner Falle

    [SerializeField] private ARRaycastManager raycastManager;
    private GameObject spawnedBait;

    // Eine Liste, in der AR Foundation die Trefferpunkte speichert
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private PlayerInputSystem _playerInput;

    public static event Action<Transform> OnBaitPlaced;


    void Awake()
    {
        _playerInput = new PlayerInputSystem();
    }

    void OnEnable()
    {
        // PlayerInput System anmachen
        _playerInput.Enable();
        // Funktionen mit Event-Callbacks verknüpfen
        _playerInput.Touch.PrimaryTouch.performed += OnPrimaryTouch;
    }
    void OnDisable()
    {
        // Input Sytem wieder ausmachen
        _playerInput.Disable();

        // Funktionen von Event-Callbacks lösen, um keine leeren Referenzen beizubehalten
        _playerInput.Touch.PrimaryTouch.performed -= OnPrimaryTouch;
    }

    void TouchHandling()
    {
        // Der AR-Raycast: Wir schießen einen Strahl von der Touch-Position auf dem Display
        // in die echte Welt. Wir wollen NUR echte, erkannte Flächen treffen (PlaneWithinPolygon).
        Vector2 touchPosition = _playerInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>();
        bool hasHitSomething = raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);

        if (hasHitSomething)
        {
            // hits[0] ist immer der Treffer, der der Kamera am nächsten ist
            Pose hitPose = hits[0].pose;

            if (spawnedBait == null)
            {
                // Köder existiert noch nicht? Neu spawnen!
                spawnedBait = Instantiate(baitPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                // Optional: Wenn schon ein Köder da ist, zerstören wir den alten, und erzeugen einen neuen
                Destroy(spawnedBait);
                spawnedBait = Instantiate(baitPrefab, hitPose.position, hitPose.rotation);
            }

            // Event auslösen
            OnBaitPlaced?.Invoke(spawnedBait.transform);
        }
    }

    void OnPrimaryTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touched");
        TouchHandling();
    }




}
