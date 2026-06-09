using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
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



    void Start()
    {
        Invoke("InitialSpawns", 3f);
    }


    void InitialSpawns()
    {
        for (int i = 0; i < _numberOfSpawns; i++)
        {
            MapCreature newCreature = Instantiate(_creaturePrefab, _parentTransform).GetComponent<MapCreature>();
            // MapCreature SpawnCreature aufrufen mit Position an der gespawnt werden soll und 
            // einem zufälligen CreatureType das gespawnt werden soll
            newCreature.GetComponent<MapCreature>().SpawnCreature(GetRandomPointAroundPlayer());

            // Drehen der Creature, damit sie nach unten schauen
            newCreature.transform.rotation = Quaternion.Euler(0, 180, 0);
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

        // Umrechnung Meter → GPS °
        var lat = _gpsLocation.GetPlayerCoordinates().x + (random.y / _metersPerDegreeLat);
        var lon = _gpsLocation.GetPlayerCoordinates().y + (random.x / _metersPerDegreeLong);

        return new Vector2(lat, lon);
    }

}
