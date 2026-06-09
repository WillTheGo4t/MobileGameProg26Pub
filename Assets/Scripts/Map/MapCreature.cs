using UnityEngine;
using CesiumForUnity;
using Unity.Mathematics;

public class MapCreature : MonoBehaviour
{
    [SerializeField] HeightAdjuster _heightAdjuster;
    [SerializeField] CesiumGlobeAnchor _cesiumGlobeAnchor;

    public void SpawnCreature(Vector2 coordinates)
    {
        _cesiumGlobeAnchor.longitudeLatitudeHeight = new double3(coordinates.y, coordinates.x, 0);
        _cesiumGlobeAnchor.Sync();

        _heightAdjuster.AdjustHeight();
    }
}
