using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] GameObject _pooledObject;
    [SerializeField] int _poolSize;

    Queue<GameObject> _pool = new Queue<GameObject>();




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InitalizePool();
    }

    void InitalizePool()
    {
        // Für die Größe der Poolsize 
        for (int i = 0; i < _poolSize; i++)
        {
            //jeweils ein neues Objekt erstellen
            GameObject newPooledObject = Instantiate(_pooledObject, this.transform.position, this.transform.rotation) as GameObject;
            // Dem Pooled Object diesen Pool zuweisen
            newPooledObject.GetComponent<PooledObject>().SetActiveSelf(false);
            newPooledObject.GetComponent<PooledObject>().Initialize(this);
            //dann das Objekt in die Queue einfügen
            _pool.Enqueue(newPooledObject);
        }
    }

    public GameObject GetPooledObject()
    {

        GameObject newPooledObject;

        //wenn der Pool leer ist, dann wie am Start einfach ein neues anlegen.
        if (_pool.Count == 0)
        {
            //jeweils ein neues Objekt erstellen
            newPooledObject = Instantiate(_pooledObject, this.transform.position, this.transform.rotation) as GameObject;
            // Dem Pooled Object diesen Pool zuweisen
            newPooledObject.GetComponent<PooledObject>().Initialize(this);
        }
        else
        {
            //wenn der Pool nicht leer ist, aus dem Pool nehmen
            newPooledObject = _pool.Dequeue();
        }


        newPooledObject.GetComponent<PooledObject>().SetActiveSelf(true);
        return newPooledObject;
    }

    public void ReturnPooledObject(GameObject pooledObject)
    {
        pooledObject.GetComponent<PooledObject>().SetActiveSelf(false);
        _pool.Enqueue(pooledObject);
    }

}
