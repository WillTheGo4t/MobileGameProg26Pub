using UnityEngine;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine.EventSystems;

public class MapFruit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] HeightAdjuster _heightAdjuster;
    [SerializeField] CesiumGlobeAnchor _cesiumGlobeAnchor;
    [SerializeField] GameObject _inRangeVisuals;
    [SerializeField] float _catchingRange;
    [SerializeField] Transform _visualRoot;
    [SerializeField] GameObject _fruitModelPrefab;

    [SerializeField] Vector3 _fruitScale = new Vector3(3f, 3f, 3f);

    bool isInCatchingRange;
    public bool IsInCatchingRange => isInCatchingRange;

    public void SpawnFruit(Vector2 coordinates)
    {
        if (_fruitModelPrefab != null)
        {
            GameObject visual = Instantiate(_fruitModelPrefab, _visualRoot);
            visual.transform.localScale = _fruitScale;
        }

        _cesiumGlobeAnchor.longitudeLatitudeHeight = new double3(coordinates.y, coordinates.x, 0);
        _cesiumGlobeAnchor.Sync();

        if (_heightAdjuster != null)
            _heightAdjuster.AdjustHeight();
    }

    void Update()
    {
        CalculateDistanceToPlayer();
    }

    void CalculateDistanceToPlayer()
    {
        Vector2 fruit = new Vector2(this.transform.position.x, this.transform.position.z);
        float dist = fruit.magnitude;

        if (dist <= _catchingRange)
        {
            isInCatchingRange = true;
            SetCatchingVisualsActive(true);
        }
        else
        {
            isInCatchingRange = false;
            SetCatchingVisualsActive(false);
        }
    }

    void SetCatchingVisualsActive(bool value)
    {
        if (_inRangeVisuals != null)
            _inRangeVisuals.SetActive(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInCatchingRange)
            return;

        GameplayManager.Instance.AddFruit(1);
        Destroy(gameObject);
    }
}