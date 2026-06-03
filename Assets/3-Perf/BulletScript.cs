using UnityEngine;

public class BulletScript : MonoBehaviour
{

     [SerializeField] PooledObject _pooledObject;

     //wenn wir neu aktiviert werden, dann neu die Zerstörung starten
    void OnEnable()
    {
        Invoke("DestroySelf", 3f);
    }

    void DestroySelf()
    {
        _pooledObject.ReturnToPool();
    }
}
