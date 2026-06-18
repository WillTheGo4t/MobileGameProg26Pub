using UnityEngine;

public class ARCage : MonoBehaviour
{
    [SerializeField] float _catchRadius = 0.3f;
    [SerializeField] float _baitSnapRadius = 0.8f;

    void Update()
    {
        GameObject bait = GameObject.FindGameObjectWithTag("Bait");
        if (bait != null && Vector3.Distance(transform.position, bait.transform.position) <= _baitSnapRadius)
        {
            bait.transform.position = this.transform.position;
        }

        ARCreature creature = FindAnyObjectByType<ARCreature>();
        if (creature != null && Vector3.Distance(transform.position, creature.transform.position) <= _catchRadius)
        {
            creature.CatchCreature();
            Destroy(gameObject);
        }
    }
}