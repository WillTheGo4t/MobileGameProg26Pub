using UnityEngine;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapCreature : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] HeightAdjuster _heightAdjuster;
    [SerializeField] CesiumGlobeAnchor _cesiumGlobeAnchor;

    [SerializeField] GameObject _inRangeVisuals;
    [SerializeField] float _catchingRange;

    [SerializeField] Transform _visualRoot;

    ScriptableCreature _scriptableCreature;

    bool isInCatchingRange;

    public void SpawnCreature(Vector2 coordinates, ScriptableCreature scriptableCreatureToSpawn)
    {
        _scriptableCreature = scriptableCreatureToSpawn;

        GameObject creatureVisuals = Instantiate (_scriptableCreature.Model, _visualRoot);

        _cesiumGlobeAnchor.longitudeLatitudeHeight = new double3(coordinates.y, coordinates.x, 0);
        _cesiumGlobeAnchor.Sync();

        _heightAdjuster.AdjustHeight();
    }


    void Update()
    {
        CalculateDistanceToPlayer();
    }


    void CalculateDistanceToPlayer()
    {
        Vector2 creature = new Vector2(this.transform.position.x, this.transform.position.z);
        float dist = creature.magnitude;

        if (dist <= _catchingRange)
        {
            isInCatchingRange=true;
            SetCatchingVisualsActive(true);
        }
        else
        {
            isInCatchingRange=false;
            SetCatchingVisualsActive(false);
        }

    }

    void SetCatchingVisualsActive(bool value)
    {
        _inRangeVisuals.SetActive(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isInCatchingRange)
            return;


        GameplayManager.Instance.SetSelectedCreature (_scriptableCreature);
        SceneManager.LoadScene("CreatureCatch");
    }
}
