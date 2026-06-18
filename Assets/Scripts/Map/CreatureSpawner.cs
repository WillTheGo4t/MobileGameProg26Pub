using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    float _metersPerDegreeLong;
    const float _metersPerDegreeLat = 111320f; // 111.320m = 111,32km 
    [SerializeField] GPSLocation _gpsLocation;
    [SerializeField] GameObject _creaturePrefab;
    // in Unity Units = Meter
    [SerializeField] float _radius;
    // GameObject that is Child of CesiumGeoReference so we spawn the Creatures correctly.
    [SerializeField] Transform _parentTransform;
    [SerializeField] float _numberOfSpawns = 5;

    [SerializeField] List<ScriptableCreature> _possibleSpawnableCreatures;

    private List<MapCreature> _spawnedCreatures = new List<MapCreature>();

    void Start()
    {
        InvokeRepeating("RefreshSpawns", 3f, 30f);
    }

    public void ForceRefresh()
    {
        RefreshSpawns();
    }

    void RefreshSpawns()
    {
        for (int i = _spawnedCreatures.Count - 1; i >= 0; i--)
        {
            if (_spawnedCreatures[i] == null)
            {
                _spawnedCreatures.RemoveAt(i);
                continue;
            }

            if (!_spawnedCreatures[i].IsInCatchingRange)
            {
                Destroy(_spawnedCreatures[i].gameObject);
                _spawnedCreatures.RemoveAt(i);
            }
        }

        int creaturesToSpawn = (int)_numberOfSpawns - _spawnedCreatures.Count;

        for (int i = 0; i < creaturesToSpawn; i++)
        {
            // MapCreature SpawnCreature aufrufen mit Position an der gespawnt werden soll und 
            // einem zufälligen CreatureType das gespawnt werden soll

            int random = Random.Range(0, _possibleSpawnableCreatures.Count);
            GameObject newCreature = Instantiate(_creaturePrefab, _parentTransform);

            MapCreature mapCreature = newCreature.GetComponent<MapCreature>();
            mapCreature.SpawnCreature(GetRandomPointAroundPlayer(), _possibleSpawnableCreatures[random]);

            // Drehen der Creature, damit sie nach unten schauen
            newCreature.transform.rotation = Quaternion.Euler(0, 180, 0);

            _spawnedCreatures.Add(mapCreature);
        }
    }

    Vector2 GetRandomPointAroundPlayer()
    {
        // Zufälliger Punkt im Kreis
        Vector2 random = Random.insideUnitCircle * _radius;

        // Umberechnung der MeterProGrad Longitude, alternativ auch einfach 111320f nehmen
        _metersPerDegreeLong = 111320f
        * Mathf.Cos(_gpsLocation.GetPlayerCoordinates().x
        * Mathf.Deg2Rad);

        // Umrechnung Meter -> GPS °
        var lat = _gpsLocation.GetPlayerCoordinates().x + (random.y / _metersPerDegreeLat);
        var lon = _gpsLocation.GetPlayerCoordinates().y + (random.x / _metersPerDegreeLong);

        return new Vector2(lat, lon);
    }
}
