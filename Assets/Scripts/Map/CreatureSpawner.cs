using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    float _metersPerDegreeLong;
    const float _metersPerDegreeLat = 111320f;
    [SerializeField] GPSLocation _gpsLocation;

    [Header("Creatures")]
    [SerializeField] GameObject _creaturePrefab;
    [SerializeField] float _numberOfSpawns = 5;
    [SerializeField] List<ScriptableCreature> _possibleSpawnableCreatures;

    [Header("Fruits")]
    [SerializeField] GameObject _fruitPrefab;
    [SerializeField] int _maxFruits = 5;

    [Header("General Settings")]
    [SerializeField] float _radius;
    [SerializeField] Transform _parentTransform;

    private List<MapCreature> _spawnedCreatures = new List<MapCreature>();
    private List<MapFruit> _spawnedFruits = new List<MapFruit>();

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

        for (int i = _spawnedFruits.Count - 1; i >= 0; i--)
        {
            if (_spawnedFruits[i] == null)
            {
                _spawnedFruits.RemoveAt(i);
                continue;
            }

            if (!_spawnedFruits[i].IsInCatchingRange)
            {
                Destroy(_spawnedFruits[i].gameObject);
                _spawnedFruits.RemoveAt(i);
            }
        }

        // 3. Neue Kreaturen spawnen
        int creaturesToSpawn = (int)_numberOfSpawns - _spawnedCreatures.Count;
        for (int i = 0; i < creaturesToSpawn; i++)
        {
            int random = Random.Range(0, _possibleSpawnableCreatures.Count);
            GameObject newCreature = Instantiate(_creaturePrefab, _parentTransform);

            MapCreature mapCreature = newCreature.GetComponent<MapCreature>();
            mapCreature.SpawnCreature(GetRandomPointAroundPlayer(), _possibleSpawnableCreatures[random]);

            newCreature.transform.rotation = Quaternion.Euler(0, 180, 0);
            _spawnedCreatures.Add(mapCreature);
        }

        // 4. Neue Früchte spawnen
        int fruitsToSpawn = _maxFruits - _spawnedFruits.Count;
        for (int i = 0; i < fruitsToSpawn; i++)
        {
            GameObject newFruit = Instantiate(_fruitPrefab, _parentTransform);

            MapFruit mapFruit = newFruit.GetComponent<MapFruit>();
            mapFruit.SpawnFruit(GetRandomPointAroundPlayer());

            _spawnedFruits.Add(mapFruit);
        }
    }

    Vector2 GetRandomPointAroundPlayer()
    {
        Vector2 random = Random.insideUnitCircle * _radius;
        _metersPerDegreeLong = 111320f * Mathf.Cos(_gpsLocation.GetPlayerCoordinates().x * Mathf.Deg2Rad);
        var lat = _gpsLocation.GetPlayerCoordinates().x + (random.y / _metersPerDegreeLat);
        var lon = _gpsLocation.GetPlayerCoordinates().y + (random.x / _metersPerDegreeLong);
        return new Vector2(lat, lon);
    }
}