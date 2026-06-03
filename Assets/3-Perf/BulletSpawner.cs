using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] ObjectPool _objectPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnBullets",0f,1f);
    }

    void SpawnBullets()
    {
        GameObject newBullet = _objectPool.GetPooledObject();
        newBullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100f);
    }

 
}
