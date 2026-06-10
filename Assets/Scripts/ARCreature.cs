using Unity.VisualScripting;
using UnityEngine;

public class ARCreature : MonoBehaviour
{
    bool isMoving;
    Transform targetBait;
    [SerializeField] float stopDistance = 0.1f;
    [SerializeField] float speed = 10f;

    void OnDisable()
    {
        // Unbedingt immer desubscriben, um Memory Leaks zu verhindern!
        ARBaitSpawner.OnBaitPlaced -= HandleNewBait;
    }

    void OnEnable()
    {
        // Auf das statische Event vom BaitSpawner subscriben
        ARBaitSpawner.OnBaitPlaced += HandleNewBait;
    }

    private void HandleNewBait(Transform baitTransform)
    {
        targetBait = baitTransform;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Wenn wir kein Ziel haben oder nicht laufen sollen, tun wir nichts
        if (!isMoving || targetBait == null) return;

        // 1. Blickrichtung zum Köder drehen (sieht natürlicher aus)
        Vector3 targetPosition = new Vector3(targetBait.position.x,
        transform.position.y,
        targetBait.position.z);

        transform.LookAt(targetPosition);

        // 2. Auf den Köder zubewegen
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 3. Distanz prüfen: Sind wir nah genug dran?
        if (Vector3.Distance(transform.position, targetPosition) <= stopDistance)
        {
            //CatchLogic();
            Debug.Log("Caught!");
        }
    }



}
