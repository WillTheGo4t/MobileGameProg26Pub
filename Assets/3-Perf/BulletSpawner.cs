using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] GameObject _bullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnBullets",0f,1f);
    }

    void SpawnBullets()
    {
        GameObject newBullet = Instantiate (_bullet, this.transform.position, this.transform.rotation) as GameObject;
        newBullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100f);
    }

 
}
