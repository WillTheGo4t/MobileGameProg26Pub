using UnityEngine;
using System.Collections;
using TMPro;

public class GPS_Script : MonoBehaviour
{
    [SerializeField] private TMP_Text locationText;

    public enum GPSMode { Idle, Walking, Encounter }
    private GPSMode currentMode = GPSMode.Idle;

    // Mock-Variablen
    private bool isMocking = false;
    private double mockLatitude = 52.5200;  // Startpunkt Berlin
    private double mockLongitude = 13.4050;

    // Erdradius für die präzise Meter-zu-Koordinaten-Berechnung (WGS84 Annäherung)
    private const double EarthRadius = 6378137.0;

    void Start()
    {
        SetGPSMode(GPSMode.Idle);
    }

    public void SetGPSMode(GPSMode newMode)
    {
        currentMode = newMode;
        if (!isMocking)
        {
            if (Input.location.status == LocationServiceStatus.Running || Input.location.status == LocationServiceStatus.Initializing)
            {
                Input.location.Stop();
            }
            StopAllCoroutines();
            StartCoroutine(StartLocationServiceWithParams(100f, 50f)); // Beispielwerte
        }
    }

    IEnumerator StartLocationServiceWithParams(float accuracy, float distance)
    {
        // (Deine bekannten Android-Permission- und Aktivierungsprüfungen hier...)
        Input.location.Start(accuracy, distance);

        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }

        while (Input.location.status == LocationServiceStatus.Running && !isMocking)
        {
            UpdateUI(Input.location.lastData.latitude, Input.location.lastData.longitude, "Echtes GPS");
            yield return new WaitForSeconds(1);
        }
    }

    // Zentrale Methode zur UI-Aktualisierung
    private void UpdateUI(double lat, double lng, string source)
    {
        locationText.text = $"[{source} - MODUS: {currentMode}]\n" +
                            $"Latitude: {lat:F6}\n" +
                            $"Longitude: {lng:F6}\n" +
                            $"Mocking aktiv: {isMocking}";
    }

    // --- MOCK BUTTON FUNKTIONEN ---

    // 1. Feste Locations anspringen
    public void SetMockLocation(int locationIndex)
    {
        isMocking = true;
        StopAllCoroutines(); // Stoppt den echten GPS-Auslese-Loop

        switch (locationIndex)
        {
            case 1: // Brandenburger Tor
                mockLatitude = 52.516275;
                mockLongitude = 13.377704;
                break;
            case 2: // Kölner Dom
                mockLatitude = 50.941278;
                mockLongitude = 6.958281;
                break;
            case 3: // Eiffelturm
                mockLatitude = 48.858370;
                mockLongitude = 2.294481;
                break;
        }
        UpdateUI(mockLatitude, mockLongitude, "Mock-Fixpunkt");
    }

    // 2. Bewegung simulieren (2 Meter nach Norden)
    public void WalkNorth()
    {
        isMocking = true;
        // Berechnung: Verschiebung in Metern auf dem Breitengrad
        double deltaLat = 2.0 / EarthRadius;
        mockLatitude += deltaLat * (180.0 / System.Math.PI);

        UpdateUI(mockLatitude, mockLongitude, "Mock-Bewegung (Nord)");
    }

    // 3. Bewegung simulieren (2 Meter nach Osten)
    public void WalkEast()
    {
        isMocking = true;
        // Berechnung: Verschiebung in Metern auf dem Längengrad (abhängig von der aktuellen Breite)
        double radLat = mockLatitude * (System.Math.PI / 180.0);
        double deltaLng = 2.0 / (EarthRadius * System.Math.Cos(radLat));
        mockLongitude += deltaLng * (180.0 / System.Math.PI);

        UpdateUI(mockLatitude, mockLongitude, "Mock-Bewegung (Ost)");
    }

    // Zurück zum echten GPS schalten
    public void DisableMocking()
    {
        isMocking = false;
        SetGPSMode(currentMode);
    }
}