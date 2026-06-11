using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCreatureSpawner : MonoBehaviour
{
    [SerializeField] ARPlaneManager _arPlaneManager;

    [SerializeField] GameObject _creaturePrefab;

    [SerializeField] float _scanTime;


    void Start()
    {
        Invoke("SpawnCreature", _scanTime);
    }

    void SpawnCreature()
    {
        ARPlane bestPlane = null;
        float largestArea = 0f;

        // Wir gehen durch alle aktuell erkannten Flächen durch
        foreach (var plane in _arPlaneManager.trackables)
        {
            // Die Größe der Fläche berechnen (Breite * Länge des Extents)
            float planeArea = plane.extents.x * plane.extents.y;

            if (planeArea > largestArea)
            {
                largestArea = planeArea;
                bestPlane = plane;
            }
        }


        if (bestPlane == null)
        {
            Debug.Log("Keine Plane gefunden. Starte Suche nach 5 Sekunden neu...");
            Invoke("SpawnCreature", _scanTime);
        }
        else
        {
            CreateARCreature(bestPlane);
        }

    }


    void CreateARCreature(ARPlane bestPlane)
    {
        // Null Checks
        if (GameplayManager.Instance == null)
        {
            Debug.LogError("GameplayManager Instance not set");
            return;
        }

        if (GameplayManager.Instance.SelectedCreature == null)
        {
            Debug.LogError("SelectedCreature not set");
            return;
        }

        GameObject spawnedCreature = Instantiate(_creaturePrefab, bestPlane.center, Quaternion.identity);
        spawnedCreature.GetComponent<ARCreature>().SetCreatureType(GameplayManager.Instance.SelectedCreature);

        spawnedCreature.transform.LookAt(Camera.main.transform);
        spawnedCreature.transform.rotation = Quaternion.Euler(0, spawnedCreature.transform.rotation.eulerAngles.y, 0);
        spawnedCreature.AddComponent<ARAnchor>();

        DisablePlaneVisuals();
    }

    void DisablePlaneVisuals()
    {
        _arPlaneManager.enabled = false;
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }



}
