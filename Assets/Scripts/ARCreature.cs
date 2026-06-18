using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARCreature : MonoBehaviour
{
    bool isMoving;
    Transform targetBait;

    [SerializeField] float stopDistance = 0.1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float maxChaseDistance = 0.3f;

    public float minCageDistance = 1.0f;

    [SerializeField] Transform _visualRoot;
    [SerializeField] LineRenderer _chaseCircleRenderer;
    [SerializeField] LineRenderer _cageCircleRenderer;

    ScriptableCreature _scriptableCreature;

    void OnDisable()
    {
        ARBaitSpawner.OnBaitPlaced -= HandleNewBait;
    }

    void OnEnable()
    {
        ARBaitSpawner.OnBaitPlaced += HandleNewBait;
    }

    public void SetCreatureType(ScriptableCreature scriptableCreature)
    {
        _scriptableCreature = scriptableCreature;
        Instantiate(scriptableCreature.Model, _visualRoot);

        DrawCircles();
    }

    void DrawCircles()
    {
        if (_chaseCircleRenderer != null)
        {
            SetupLineRenderer(_chaseCircleRenderer, maxChaseDistance, Color.red);
        }

        if (_cageCircleRenderer != null)
        {
            SetupLineRenderer(_cageCircleRenderer, minCageDistance, Color.green);
        }
    }

    void SetupLineRenderer(LineRenderer lr, float radius, Color color)
    {
        int segments = 50;
        lr.positionCount = segments + 1;
        lr.useWorldSpace = false;
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;

        float angle = 0f;
        for (int i = 0; i < (segments + 1); i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lr.SetPosition(i, new Vector3(x, 0.01f, z));
            angle += (360f / segments);
        }
    }

    private void HandleNewBait(Transform baitTransform)
    {
        FindClosestBait();
    }

    void Update()
    {
        if (!isMoving || targetBait == null) return;

        Vector3 targetPosition = new Vector3(targetBait.position.x,
        transform.position.y,
        targetBait.position.z);

        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= stopDistance)
        {
            isMoving = false;

            if (targetBait != null)
            {
                targetBait.tag = "Untagged";
                Destroy(targetBait.gameObject);
                targetBait = null;
            }

            FindClosestBait();
        }
    }

    void FindClosestBait()
    {
        GameObject[] allBaits = GameObject.FindGameObjectsWithTag("Bait");
        float closestDistance = maxChaseDistance;
        Transform bestBait = null;

        foreach (GameObject bait in allBaits)
        {
            if (bait == null) continue;

            float distanceToBait = Vector3.Distance(transform.position, bait.transform.position);

            if (distanceToBait <= closestDistance)
            {
                closestDistance = distanceToBait;
                bestBait = bait.transform;
            }
        }

        if (bestBait != null)
        {
            targetBait = bestBait;
            isMoving = true;
        }
    }

    public void CatchCreature()
    {
        isMoving = false;

        CreatureData caughtCreature = new CreatureData();
        caughtCreature.ScriptableID = _scriptableCreature.ID;
        caughtCreature.CaughtDateTime = DateTime.Now;
        caughtCreature.Level = 1;

        GameplayManager.Instance.AddCaughtCreature(caughtCreature);

        Invoke("LoadMapScene", 1f);
    }

    void LoadMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }
}