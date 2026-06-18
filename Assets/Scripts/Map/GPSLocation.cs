using System;
using System.Collections;
using CesiumForUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSLocation : MonoBehaviour
{
    public enum TrackingMode
    {
        Idle,
        Walking,
        Encounter
    }

    public TrackingMode _currentTrackingMode;

    [SerializeField] CesiumGeoreference _cesiumGeoReference;

    [SerializeField] TextMeshProUGUI _coordinatesTextField;
    [SerializeField] TextMeshProUGUI _trackingModeTextField;

    [Header("Dev Mode Controls")]
    [SerializeField] GameObject _devMovementControlGroup;
    [SerializeField] Image _walkNorthButtonImage;
    [SerializeField] Image _walkEastButtonImage;
    [SerializeField] Image _walkSouthButtonImage;
    [SerializeField] Image _walkWestButtonImage;

    [SerializeField] bool _useMockService;

    [SerializeField] HeightAdjuster _cameraHeightAdjuster;
    [SerializeField] HeightAdjuster _playerHeightAdjuster;

    [SerializeField] float _mockHeading = 0;
    [SerializeField] float steps = 1f;

    [SerializeField] PlayerAvatar _playerAvatar;

    bool _walkNorth;
    bool _walkEast;
    bool _walkSouth;
    bool _walkWest;

    public class PlayerPosition
    {
        public float latitude;
        public float longitude;
        public float altitude;
    }

    PlayerPosition _playerPosition;
    PlayerPosition _mockPosition;

    void Start()
    {
        _playerPosition = new PlayerPosition();
        _mockPosition = new PlayerPosition();
        Input.compass.enabled = true;

        SetMockPosition1();

        if (_devMovementControlGroup != null)
        {
            _devMovementControlGroup.SetActive(_useMockService);
        }

        if (_useMockService)
            InvokeRepeating("UpdatePlayerPosition", 1f, 1f);
        else
            StartCoroutine("StartLocationService");
    }

    IEnumerator StartLocationService()
    {
        Input.location.Start(10, 10);
        Debug.Log("Status " + Input.location.status);

        _currentTrackingMode = TrackingMode.Walking;
        _trackingModeTextField.text = "Trackmode " + _currentTrackingMode.ToString();

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Debug.Log("Ask for permission");
            yield return new WaitForSeconds(2f);
        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location Service not permitted by user!");
            yield break;
        }

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Running)
        {
            Debug.Log("Location ready");
            InvokeRepeating("UpdatePlayerPosition", 1f, 1f);
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Location initialization failed");
        }
    }

    public void ToggleDevMode()
    {
        _useMockService = !_useMockService;

        if (_devMovementControlGroup != null)
        {
            _devMovementControlGroup.SetActive(_useMockService);
        }

        if (_useMockService)
        {
            Input.location.Stop();
        }
        else
        {
            StartCoroutine(StartLocationService());
        }

        FindAnyObjectByType<CreatureSpawner>().ForceRefresh();
    }

    public void ToggleTrackMode()
    {
        TrackingMode newTrackMode = _currentTrackingMode;

        if (_currentTrackingMode == TrackingMode.Encounter)
            newTrackMode = TrackingMode.Idle;
        else
            newTrackMode++;

        SetTrackingMode(newTrackMode);
    }

    public void SetTrackingMode(TrackingMode trackingMode)
    {
        Input.location.Stop();

        _currentTrackingMode = trackingMode;
        switch (trackingMode)
        {
            case TrackingMode.Idle:
                Input.location.Start(100f, 50f);
                break;
            case TrackingMode.Walking:
                Input.location.Start(10f, 10f);
                break;
            case TrackingMode.Encounter:
                Input.location.Start(5f, 2f);
                break;
        }
        _trackingModeTextField.text = "Trackmode " + trackingMode.ToString();
    }

    public void SetMockPosition1()
    {
        _mockPosition.latitude = 49.41399f;
        _mockPosition.longitude = 8.65110f;
        _mockPosition.altitude = 143f;
    }

    public void SetMockPosition2()
    {
        _mockPosition.latitude = 49.377161f;
        _mockPosition.longitude = 8.6925317f;
        _mockPosition.altitude = 143f;
    }

    public void ToggleWalkNorth()
    {
        _walkNorth = !_walkNorth;
        if (_walkNorthButtonImage != null) _walkNorthButtonImage.color = _walkNorth ? Color.green : Color.white;
    }

    public void ToggleWalkEast()
    {
        _walkEast = !_walkEast;
        if (_walkEastButtonImage != null) _walkEastButtonImage.color = _walkEast ? Color.green : Color.white;
    }

    public void ToggleWalkSouth()
    {
        _walkSouth = !_walkSouth;
        if (_walkSouthButtonImage != null) _walkSouthButtonImage.color = _walkSouth ? Color.green : Color.white;
    }

    public void ToggleWalkWest()
    {
        _walkWest = !_walkWest;
        if (_walkWestButtonImage != null) _walkWestButtonImage.color = _walkWest ? Color.green : Color.white;
    }

    void UpdatePlayerPosition()
    {
        if (_useMockService)
            GetPlayerPositionFromMock();
        else
            GetPlayerPositionFromGPS();

        _coordinatesTextField.text = "lat: " + _playerPosition.latitude + "\nlong: " + _playerPosition.longitude + "\nalt :" + _playerPosition.altitude;

        _cesiumGeoReference.SetOriginLongitudeLatitudeHeight(_playerPosition.longitude,
        _playerPosition.latitude, _playerPosition.altitude);

        UpdateHeading();

        _cameraHeightAdjuster.AdjustHeight();
        _playerHeightAdjuster.AdjustHeight();
    }

    void GetPlayerPositionFromMock()
    {
        float stepSize = steps / 111320f;

        if (_walkNorth) _mockPosition.latitude += stepSize;
        if (_walkSouth) _mockPosition.latitude -= stepSize;
        if (_walkEast) _mockPosition.longitude += stepSize;
        if (_walkWest) _mockPosition.longitude -= stepSize;

        _playerPosition.latitude = _mockPosition.latitude;
        _playerPosition.longitude = _mockPosition.longitude;
        _playerPosition.altitude = _mockPosition.altitude;
    }

    void GetPlayerPositionFromGPS()
    {
        _playerPosition.latitude = Input.location.lastData.latitude;
        _playerPosition.longitude = Input.location.lastData.longitude;
        _playerPosition.altitude = Input.location.lastData.altitude;

        Debug.Log("Pos " + Input.location.lastData.latitude + " at " + Input.location.lastData.timestamp);
    }

    public Vector2 GetPlayerCoordinates()
    {
        return new Vector2(_playerPosition.latitude, _playerPosition.longitude);
    }

    void UpdateHeading()
    {
        float heading;
        if (_useMockService)
            heading = _mockHeading;
        else
            heading = Input.compass.trueHeading;

        _playerAvatar.SetLookDirection(heading);
    }
}